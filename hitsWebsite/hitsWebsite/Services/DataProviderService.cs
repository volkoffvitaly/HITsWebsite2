using hitsWebsite.Data;
using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Resources;
using System.Reflection;

namespace hitsWebsite.Services
{
    public class DataProviderService : IDataProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IList<CultureInfo> _cultures;
        private readonly IStringLocalizer<DataProviderService> _localizer;
        private readonly ResourceManager _resourceManager = new ResourceManager("hitsWebsite.Resources.Services.DataProviderService", Assembly.GetExecutingAssembly());

        //private readonly ResourceManager _resourceManager = new ResourceManager();

        public DataProviderService(ApplicationDbContext context, IOptions<RequestLocalizationOptions> locOptions, IStringLocalizer<DataProviderService> localizer)
        {
            this._context = context;
            this._cultures = locOptions.Value.SupportedUICultures;
            this._localizer = localizer;
        }

        public async Task<DynamicPage> GetDynamicPageInfo(String projectNameOfPage = null)
        {
            DynamicPage dynamicPage = null;
            Boolean isTracked = true; // to separate new entity from a tracked

            if (projectNameOfPage != null)
            {
                dynamicPage = await _context.DynamicPages
                    .Where(x => x.ProjectName == projectNameOfPage)
                    .Include(x => x.DynamicPageTranslations)
                    .FirstOrDefaultAsync();
            }

            if (dynamicPage == null)
            {
                dynamicPage = new DynamicPage()
                {
                    ProjectName = projectNameOfPage
                };

                isTracked = false;
            }

            if (dynamicPage.DynamicPageTranslations.Count < _cultures.Count)
            {
                for (var i = 0; i < _cultures.Count; i++)
                {
                    if (dynamicPage.DynamicPageTranslations.Where(x => x.Language == _cultures[i].Name.ToString()).SingleOrDefault() == default)
                    {
                        try
                        {
                            dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
                            {
                                Name = _resourceManager.GetString("DefaultPageName", _cultures[i]),
                                Description = _resourceManager.GetString("DefaultPageDescription", _cultures[i]),
                                Language = _cultures[i].Name.ToString(),
                            });
                        }
                        catch // if we need to implement a new language but .resx file wasn't updated yet.
                        {
                            dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
                            {
                                Name = _localizer.GetString("DefaultPageName"),
                                Description = _localizer.GetString("DefaultPageDescription"),
                                Language = _cultures[i].Name.ToString(),
                            });
                        }
                    }
                }

                if (!isTracked)
                {
                    await _context.DynamicPages.AddAsync(dynamicPage);
                }

                await _context.SaveChangesAsync();
            }

            return dynamicPage;
        }

        public async Task ChangeDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model)
        {
            var dynamicPage = await _context.DynamicPages.Where(x => x.ProjectName == projectNameOfPage).Include(x => x.DynamicPageTranslations).SingleOrDefaultAsync();

            if (dynamicPage != null)
            {
                dynamicPage.DynamicPageTranslations.Clear();

                for (var i = 0; i < _cultures.Count; i++)
                {
                    dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
                    {
                        Name = model.Name[i],
                        Description = model.Description[i],
                        Language = model.Language[i]
                    });
                }

                await _context.SaveChangesAsync();
            }

            return;
        }

        public async Task<List<ProfessionTranslation>> GetProfessions()
        {
            return await _context.ProfessionTranslations.Where(x => x.Language == CultureInfo.CurrentUICulture.Name).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }


        public async Task CreateProfession(ProfessionEditModel model)
        {
            var profession = new Profession()
            {
                ProfessionTranslations = new Collection<ProfessionTranslation>()
            };

            for (var i = 0; i < _cultures.Count; i++)
            {
                profession.ProfessionTranslations.Add(new ProfessionTranslation
                {
                    Name = model.Name[i],
                    Description = model.Description[i],
                    Language = model.Language[i]
                });
            }

            await _context.Professions.AddAsync(profession);
            await _context.SaveChangesAsync();
            return;
        }



        public Dictionary<String, String> GetBlock(String projectBlockName)
        {
            NameOfPageBlock block = null;

            if (projectBlockName != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
                using StreamReader streamReader = new StreamReader(fileStreamer);
                using JsonReader jsonReader = new JsonTextReader(streamReader);

                while (jsonReader.Read())
                {
                    // deserialize only when there's "[" character in the stream
                    if (jsonReader.TokenType == JsonToken.StartArray)
                    {
                        block = serializer.Deserialize<List<NameOfPageBlock>>(jsonReader)
                            .Where(x => x.ProjectName == projectBlockName)
                            .FirstOrDefault();
                    }
                }
            }

            if (projectBlockName == null || block == null) // протестить
            {
                var unexpectedBlockName = new Dictionary<String, String>();

                for (var i = 0; i < _cultures.Count; i++)
                {
                    unexpectedBlockName.Add(_cultures[i].Name, _localizer.GetString("UnexpectedProjectBlockName"));
                }

                return unexpectedBlockName;
            }

            return block.Translations;
        }

        // EDIT
        private List<NameOfPageBlock> GetMainPageBlocks()
        {
            List<NameOfPageBlock> blocks = new List<NameOfPageBlock>();

            JsonSerializer serializer = new JsonSerializer();
            using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
            using StreamReader streamReader = new StreamReader(fileStreamer);
            using JsonReader jsonReader = new JsonTextReader(streamReader);

            while (jsonReader.Read())
            {
                // deserialize only when there's "[" character in the stream
                if (jsonReader.TokenType == JsonToken.StartArray)
                {
                    blocks = serializer.Deserialize<List<NameOfPageBlock>>(jsonReader);
                }
            }

            return blocks;
        }

        public async void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model)
        {
            var blocks = GetMainPageBlocks();

            if (blocks.Count > 0)
            {
                using (FileStream fs = new FileStream("NameOfPageBlock.json", FileMode.Create))
                {
                    var block = blocks.Where(x => x.ProjectName == projectBlockName).FirstOrDefault();
                    var newTranslations = new Dictionary<String, String>();
                    for (var i = 0; i < _cultures.Count; i++)
                    {
                        newTranslations.Add(model.Language[i], model.NewBlockName[i]);
                    }

                    block.Translations = newTranslations;

                    await System.Text.Json.JsonSerializer.SerializeAsync<List<NameOfPageBlock>>(fs, blocks);
                }
            }
        }
    }
}
