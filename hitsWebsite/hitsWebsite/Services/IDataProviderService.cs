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
        public Task<DynamicPageModel> GetDynamicPageInfo(String name);
        public Task ChangeDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model);
        public Task<List<ProfessionTranslationModel>> GetProfessions();
        public Task<List<FeatureTranslationModel>> GetFeatures();
        public Task CreateProfession(ProfessionEditModel model);
        public Task CreateFeature(FeatureEditModel model);


        public Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default);
        public void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model);
    }
}
