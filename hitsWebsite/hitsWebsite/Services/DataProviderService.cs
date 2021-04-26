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

namespace hitsWebsite.Services
{
    public class DataProviderService : IDataProviderService
    {
        private ApplicationDbContext _context;
        private readonly IList<CultureInfo> _cultures;
        
        public DataProviderService(ApplicationDbContext context, IOptions<RequestLocalizationOptions> locOptions)
        {
            this._context = context;
            this._cultures = locOptions.Value.SupportedUICultures;
        }

        public async Task<List<ProfessionTranslation>> GetProfessions()
        {
            return await _context.ProfessionTranslations.Where(x => x.Language == CultureInfo.CurrentUICulture.Name).ToListAsync();
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
    }
}
