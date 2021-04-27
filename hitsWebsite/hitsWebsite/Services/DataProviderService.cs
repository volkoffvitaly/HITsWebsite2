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

namespace hitsWebsite.Services
{
    public class DataProviderService : IDataProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IList<CultureInfo> _cultures;

        public DataProviderService(ApplicationDbContext context, IOptions<RequestLocalizationOptions> locOptions)
        {
            this._context = context;
            this._cultures = locOptions.Value.SupportedUICultures;
        }

        public async Task<DynamicPageTranslation> GetDynamicPageInfo(String projectNameOfPage = null)
        {
            DynamicPageTranslation translation = default;

            if (projectNameOfPage != null)
            {
                translation = await _context.DynamicPageTranslations
                    .Where(x => x.Language == CultureInfo.CurrentUICulture.Name && x.DynamicPage.ProjectName == projectNameOfPage)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }

            if (translation == default)
            {
                var dynamicPage = await _context.DynamicPages.Where(x => x.ProjectName == projectNameOfPage).Include(x => x.DynamicPageTranslations).SingleOrDefaultAsync();

                translation = new DynamicPageTranslation
                {
                    DynamicPage = dynamicPage,
                    DynamicPageId = dynamicPage.Id,
                    Name = "---",
                    Description = "---",
                    Language = CultureInfo.CurrentUICulture.Name.ToString()
                };

                await _context.DynamicPageTranslations.AddAsync(translation);
                await _context.SaveChangesAsync();
            }

            return translation;
        }


        public async Task<List<ProfessionTranslation>> GetProfessions()
        {
            return await _context.ProfessionTranslations.Where(x => x.Language == CultureInfo.CurrentUICulture.Name).AsNoTracking().ToListAsync();
        }


        public async Task<int> CreateProfession(ProfessionEditModel model)
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

            return await _context.SaveChangesAsync();
        }



        public Dictionary<String, String> GetBlock(String projectBlockName)
        {
            NameOfPageBlock block = null;

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
                    var a = new Dictionary<String, String>();
                    for (var i = 0; i < block.Translations.Count; i++)
                    {
                        a.Add(model.Language[i], model.NewBlockName[i]);
                    }

                    block.Translations = a;

                    await System.Text.Json.JsonSerializer.SerializeAsync<List<NameOfPageBlock>>(fs, blocks);
                }
            }
        }
    }
}
