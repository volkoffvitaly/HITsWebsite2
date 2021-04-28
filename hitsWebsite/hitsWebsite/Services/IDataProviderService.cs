using hitsWebsite.Data;
using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Services
{
    public interface IDataProviderService
    {
        public Task<DynamicPage> GetDynamicPageInfo(String name);
        public Task ChangeDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model);
        public Task<List<ProfessionTranslation>> GetProfessions();
        public Task<int> CreateProfession(ProfessionEditModel model);


        public Dictionary<String, String> GetBlock(String projectBlockName);
        public void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model);
    }
}
