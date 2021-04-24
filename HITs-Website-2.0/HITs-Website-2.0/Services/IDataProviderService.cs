using HITs_Website_2._0.Data;
using HITs_Website_2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HITs_Website_2._0.Services
{
    public interface IDataProviderService
    {
        public Task<List<Profession>> GetProfessions();
    }
}
