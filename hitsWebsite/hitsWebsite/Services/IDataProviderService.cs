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
        // DB

        #region *RU* DynamicPage
        public Task<DynamicPage> GetDynamicPage(String name);
        public Task ChangeDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model);
        #endregion


        #region CRUD Professions
        public Task CreateProfession(ProfessionEditModel model);
        public Task<List<Profession>> GetProfessions();
        public Task EditProfession(String id, ProfessionEditModel model);
        public Task DeleteProfession(String id);
        #endregion


        #region CRUD Features
        public Task CreateFeature(FeatureEditModel model);
        public Task<List<Feature>> GetFeatures();
        public Task EditFeature(String id, FeatureEditModel model);
        public Task DeleteFeature(String id);
        #endregion


        #region CRUD Humans
        public Task CreateHuman(HumanCreateModel model);
        public Task<List<Human>> GetTeachers();  // Retrive part of all Humans by "post" 
        public Task<List<Human>> GetGraduates(); // Retrive part of all Humans by "post" 
        public Task EditHuman(String id, HumanEditModel model);
        public Task DeleteHuman(String id);
        #endregion


        #region CRUD Academic Subjects
        public Task CreateAcademicSubject(AcademicSubjectEditModel model);
        public Task<List<AcademicSubject>> GetAcademicSubjects();
        public Task EditAcademicSubject(String id, AcademicSubjectEditModel model);
        public Task DeleteAcademicSubject(String id);
        #endregion


        #region CRUD City Features
        public Task<List<CityFeature>> GetCityFeatures();
        public Task<List<CityFeature>> GetCityFeaturesWithPhotos();
        public Task CreateCityFeature(CityFeatureEditModel model);
        public Task EditCityFeature(String id, CityFeatureEditModel model);
        public Task DeleteCityFeature(String id);
        #endregion


        #region CRUD Dormitory
        public Task<List<Dormitory>> GetDormitories();
        public Task CreateDormitory(DormitoryCreateModel model);
        public Task EditDormitory(String id, DormitoryEditModel model);
        public Task DeleteDormitory(String id);
        #endregion

        // JSON

        #region *RU* Block
        public Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default);
        public void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model);
        #endregion
    }
}
