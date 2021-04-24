using HITs_Website_2._0.Data;
using HITs_Website_2._0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HITs_Website_2._0.Services
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
            return await _context.Professions.ToListAsync();
        }
    }
}
