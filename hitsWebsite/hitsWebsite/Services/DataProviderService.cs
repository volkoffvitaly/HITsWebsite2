using hitsWebsite.Data;
using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Services
{
    public class DataProviderService: IDataProviderService
    {
        private ApplicationDbContext _context;

        public DataProviderService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Profession>> GetProfessions()
        {
            return await _context.Professions.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateProfession(ProfessionEditModel model)
        {
            var profession = new Profession()
            {
                Name = model.Name,
                Description = model.Description
            };

            await _context.Professions.AddAsync(profession);

            return await _context.SaveChangesAsync();
        }
    }
}
