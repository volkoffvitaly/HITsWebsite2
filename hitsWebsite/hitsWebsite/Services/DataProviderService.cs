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

        public DataProviderService(ApplicationDbContext context, IOptions<RequestLocalizationOptions> locOptions, IStringLocalizer<DataProviderService> localizer)
        {
            _context = context;
            _cultures = locOptions.Value.SupportedUICultures;
            _localizer = localizer;
        }


        #region DB



        #region DynamicPage

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

            if (dynamicPage.DynamicPageTranslations.Count >= _cultures.Count)
            {
                return dynamicPage;
            }

            foreach (var culture in _cultures)
            {
                if (dynamicPage.DynamicPageTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default)
                {
                    try
                    {
                        dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
                        {
                            Name = _resourceManager.GetString("DefaultPageName", culture),
                            Description = _resourceManager.GetString("DefaultPageDescription", culture),
                            Language = culture.Name.ToString(),
                        });
                    }
                    catch // if we need to implement a new language but .resx file wasn't updated yet.
                    {
                        dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
                        {
                            Name = _localizer.GetString("DefaultPageName"),
                            Description = _localizer.GetString("DefaultPageDescription"),
                            Language = culture.Name.ToString(),
                        });
                    }
                }
            }


            if (!isTracked)
            {
                await _context.DynamicPages.AddAsync(dynamicPage);
            }

            await _context.SaveChangesAsync();

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

        #endregion


        #region GetListElements

        public async Task<List<Profession>> GetProfessions()
        {
            return await _context.Professions.Include(x => x.ProfessionTranslations).AsNoTracking().ToListAsync();
        }
        public async Task<List<ProfessionTranslation>> GetProfessions_CurrentCulture()
        {
            return await _context.ProfessionTranslations.Where(x => x.Language == CultureInfo.CurrentUICulture.Name).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }
        public async Task<List<FeatureTranslation>> GetFeatures()
        {
            return await _context.FeatureTranslations.Where(x => x.Language == CultureInfo.CurrentUICulture.Name).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        #endregion


        #region CreateListElement

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
        public async Task CreateFeature(FeatureEditModel model)
        {
            var feature = new Feature()
            {
                FeatureTranslations = new Collection<FeatureTranslation>()
            };

            for (var i = 0; i < _cultures.Count; i++)
            {
                feature.FeatureTranslations.Add(new FeatureTranslation
                {
                    Name = model.Name[i],
                    Description = model.Description[i],
                    Language = model.Language[i]
                });
            }

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return;
        }

        #endregion


        #region EditListElement

        public async Task EditProfession(String id, ProfessionEditModel model)
        {
            var profession = await _context.Professions.Where(x => x.Id.ToString() == id).Include(x => x.ProfessionTranslations).SingleOrDefaultAsync();

            if (profession == null)
            {
                return;
            }

            profession.ProfessionTranslations.Clear();

            for (var i = 0; i < model.Language.Count; i++)
            {
                profession.ProfessionTranslations.Add(new ProfessionTranslation()
                {
                    Name = model.Name[i],
                    Description = model.Description[i],
                    Language = model.Language[i]
                });
            }

            await _context.SaveChangesAsync();

            return;
        }

        #endregion


        #region DeleteListElement

        public async Task DeleteProfession(String id)
        {
            var profession = await _context.Professions.Where(x => x.Id.ToString() == id).Include(x => x.ProfessionTranslations).SingleOrDefaultAsync();
            this._context.Professions.Remove(profession);
            await this._context.SaveChangesAsync();
        }

        #endregion



        #endregion




        #region JSON
        public async Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default)
        {
            if (projectBlockName != default)
            {
                NameOfPageBlockModel block = default;

                JsonSerializer serializer = new JsonSerializer();
                using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
                using StreamReader streamReader = new StreamReader(fileStreamer);
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    while (jsonReader.Read())
                    {
                        // deserialize only when there's "[" character in the stream
                        if (jsonReader.TokenType == JsonToken.StartArray)
                        {
                            block = serializer.Deserialize<List<NameOfPageBlockModel>>(jsonReader)
                                .Where(x => x.ProjectName == projectBlockName)
                                .SingleOrDefault();
                        }
                    }
                }

                if (block == default || block.Translations.Count < _cultures.Count)
                {
                    block = await createDefaultBlockName(projectBlockName, block);
                }

                return block.Translations;
            }

            return default;
        }

        public async void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model)
        {
            var blockNames = getBlockNames();  // to reserialize all of them

            if (blockNames.Count > 0)  // can't change any name if no one block exists
            {
                var newBlockName = blockNames.Where(x => x.ProjectName == projectBlockName).FirstOrDefault();

                newBlockName.Translations.Clear();
                for (var i = 0; i < _cultures.Count; i++)
                {
                    newBlockName.Translations.Add(model.Language[i], model.NewBlockName[i]);
                }

                using FileStream fileStreamer = new FileStream("NameOfPageBlock.json", FileMode.Create);
                await System.Text.Json.JsonSerializer.SerializeAsync(fileStreamer, blockNames);
            }
        }

        #region HiddenLogic
        private async Task<NameOfPageBlockModel> createDefaultBlockName(String projectBlockName, NameOfPageBlockModel oldBlock)
        {
            var blockNames = getBlockNames();  // to reserialize all of them


            var defaultBlock = new NameOfPageBlockModel()
            {
                ProjectName = projectBlockName,
                Translations = new Dictionary<string, string>()
            };

            if (oldBlock != default)  // If some translations was into oldBlock early
            {
                blockNames.Remove(blockNames.Where(x => x.ProjectName == oldBlock.ProjectName).SingleOrDefault()); // .Remove(oldBlock) is don't work... Idk, why?
                foreach (var oldTranslation in oldBlock.Translations)
                {
                    defaultBlock.Translations.Add(oldTranslation.Key, oldTranslation.Value);
                }
            }

            foreach (var culture in _cultures)
            {
                if (!defaultBlock.Translations.TryGetValue(culture.Name, out var smthVar))
                {
                    try
                    {
                        defaultBlock.Translations.Add(culture.Name.ToString(), _resourceManager.GetString("DefaultBlockName", culture));
                    }
                    catch // if we need to implement a new language but .resx file wasn't updated yet.
                    {
                        defaultBlock.Translations.Add(culture.Name.ToString(), _localizer.GetString("DefaultBlockName"));
                    }
                }
            }

            blockNames.Add(defaultBlock);

            using FileStream fileStreamer = new FileStream("NameOfPageBlock.json", FileMode.Create);
            await System.Text.Json.JsonSerializer.SerializeAsync(fileStreamer, blockNames);

            return defaultBlock;
        }

        private List<NameOfPageBlockModel> getBlockNames()
        {
            List<NameOfPageBlockModel> blocks = new List<NameOfPageBlockModel>();

            JsonSerializer serializer = new JsonSerializer();
            using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
            using StreamReader streamReader = new StreamReader(fileStreamer);
            using JsonReader jsonReader = new JsonTextReader(streamReader);

            while (jsonReader.Read())
            {
                // deserialize only when there's "[" character in the stream
                if (jsonReader.TokenType == JsonToken.StartArray)
                {
                    blocks = serializer.Deserialize<List<NameOfPageBlockModel>>(jsonReader);
                }
            }

            return blocks;
        }
        #endregion

        #endregion
    }
}
