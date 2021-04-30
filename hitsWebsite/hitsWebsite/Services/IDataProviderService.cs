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
        public Task<List<Human>> GetTeachers();
        public Task<List<Human>> GetGraduates();
        public Task CreateHuman(HumanEditModel model);
        public Task EditHuman(String id, HumanEditModel model);
        public Task<List<AcademicSubject>> GetAcademicSubjects();
        public Task CreateProfession(ProfessionEditModel model);
        public Task EditProfession(String id, ProfessionEditModel model);
        public Task EditFeature(String id, FeatureEditModel model);
        public Task DeleteProfession(String id);
        public Task DeleteFeature(String id);
        public Task CreateFeature(FeatureEditModel model);

        public Task CreateAcademicSubject(AcademicSubjectEditModel model);
        public Task EditAcademicSubject(String id, AcademicSubjectEditModel model);
        public Task DeleteAcademicSubject(String id);


        public Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default);
        public void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model);
    }
}
