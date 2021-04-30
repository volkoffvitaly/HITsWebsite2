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
        public Task<List<Profession>> GetProfessions();
        public Task<List<Feature>> GetFeatures();
        public Task CreateProfession(ProfessionEditModel model);
        public Task EditProfession(String id, ProfessionEditModel model);
        public Task EditFeature(String id, FeatureEditModel model);
        public Task DeleteProfession(String id);
        public Task DeleteFeature(String id);
        public Task CreateFeature(FeatureEditModel model);


        public Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default);
        public void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model);
    }
}
