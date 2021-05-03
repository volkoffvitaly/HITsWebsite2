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
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Resources;
using System.Reflection;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace hitsWebsite.Services
{
	public class DataProviderService : IDataProviderService
	{
		private readonly ApplicationDbContext _context;
		private readonly IList<CultureInfo> _cultures;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IStringLocalizer<DataProviderService> _localizer;
		private readonly ResourceManager _resourceManager = new ResourceManager("hitsWebsite.Resources.Services.DataProviderService", Assembly.GetExecutingAssembly());

		public DataProviderService(ApplicationDbContext context, IOptions<RequestLocalizationOptions> locOptions, IWebHostEnvironment hostingEnvironment, IStringLocalizer<DataProviderService> localizer)
		{
			_context = context;
			_cultures = locOptions.Value.SupportedUICultures;
			_hostingEnvironment = hostingEnvironment;
			_localizer = localizer;
		}


		#region *RU* DynamicPage

		public async Task<DynamicPage> GetDynamicPage(String projectNameOfPage = null)
		{
			DynamicPage dynamicPage = null;
			Boolean isTracked = true; // to separate new entity from a tracked

			if (projectNameOfPage != null)
			{
				dynamicPage = await _context.DynamicPages
					.Where(x => x.ProjectName == projectNameOfPage)
					.Include(x => x.DynamicPageTranslations)
					.FirstOrDefaultAsync();
			}

			if (dynamicPage == null)
			{
				dynamicPage = new DynamicPage()
				{
					ProjectName = projectNameOfPage
				};

				isTracked = false;
			}

			if (dynamicPage.DynamicPageTranslations.Count >= _cultures.Count)
			{
				return dynamicPage;
			}

			foreach (var culture in _cultures)
			{
				if (dynamicPage.DynamicPageTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default)
				{
					try
					{
						dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
						{
							Name = _resourceManager.GetString("DefaultName", culture),
							Description = _resourceManager.GetString("DefaultDescription", culture),
							Language = culture.Name.ToString(),
						});
					}
					catch // if we need to implement a new language but .resx file wasn't updated yet.
					{
						dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
						{
							Name = _localizer.GetString("DefaultName"),
							Description = _localizer.GetString("DefaultDescription"),
							Language = culture.Name.ToString(),
						});
					}
				}
			}


			if (!isTracked)
			{
				await _context.DynamicPages.AddAsync(dynamicPage);
			}

			await _context.SaveChangesAsync();

			return dynamicPage;
		}
		public async Task ChangeDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model)
		{
			var dynamicPage = await _context.DynamicPages.Where(x => x.ProjectName == projectNameOfPage).Include(x => x.DynamicPageTranslations).SingleOrDefaultAsync();

			if (dynamicPage != null)
			{
				dynamicPage.DynamicPageTranslations.Clear();

				for (var i = 0; i < _cultures.Count; i++)
				{
					dynamicPage.DynamicPageTranslations.Add(new DynamicPageTranslation()
					{
						Name = model.Name[i],
						Description = model.Description[i],
						Language = model.Language[i]
					});
				}

				await _context.SaveChangesAsync();
			}

			return;
		}

		#endregion


		#region CRUD Professions
		public async Task<List<Profession>> GetProfessions()
		{
			var professions = await _context.Professions.Include(x => x.ProfessionTranslations).AsNoTracking().ToListAsync();

			foreach (var profession in professions)
			{
				if (profession.ProfessionTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (profession.ProfessionTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								profession.ProfessionTranslations.Add(new ProfessionTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								profession.ProfessionTranslations.Add(new ProfessionTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return professions;
		}

		public async Task CreateProfession(ProfessionEditModel model)
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
			await _context.SaveChangesAsync();
			return;
		}

		public async Task EditProfession(String id, ProfessionEditModel model)
		{
			var profession = await _context.Professions.Where(x => x.Id.ToString() == id).Include(x => x.ProfessionTranslations).SingleOrDefaultAsync();

			if (profession == null)
			{
				return;
			}

			profession.ProfessionTranslations.Clear();

			for (var i = 0; i < model.Language.Count; i++)
			{
				profession.ProfessionTranslations.Add(new ProfessionTranslation()
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();

			return;
		}

		public async Task DeleteProfession(String id)
		{
			var profession = await _context.Professions.Where(x => x.Id.ToString() == id).Include(x => x.ProfessionTranslations).SingleOrDefaultAsync();
			_context.Professions.Remove(profession);
			await _context.SaveChangesAsync();
		}
		#endregion


		#region CRUD Features
		public async Task<List<Feature>> GetFeatures()
		{
			var features = await _context.Features.Include(x => x.FeatureTranslations).AsNoTracking().ToListAsync();

			foreach (var feature in features)
			{
				if (feature.FeatureTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (feature.FeatureTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								feature.FeatureTranslations.Add(new FeatureTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								feature.FeatureTranslations.Add(new FeatureTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return features;
		}

		public async Task CreateFeature(FeatureEditModel model)
		{
			var feature = new Feature()
			{
				FeatureTranslations = new Collection<FeatureTranslation>()
			};

			for (var i = 0; i < _cultures.Count; i++)
			{
				feature.FeatureTranslations.Add(new FeatureTranslation
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.Features.AddAsync(feature);
			await _context.SaveChangesAsync();
			return;
		}

		public async Task EditFeature(String id, FeatureEditModel model)
		{
			var feature = await _context.Features.Where(x => x.Id.ToString() == id).Include(x => x.FeatureTranslations).SingleOrDefaultAsync();

			if (feature == null)
			{
				return;
			}

			feature.FeatureTranslations.Clear();

			for (var i = 0; i < model.Language.Count; i++)
			{
				feature.FeatureTranslations.Add(new FeatureTranslation()
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();

			return;
		}

		public async Task DeleteFeature(String id)
		{
			var feature = await _context.Features.Where(x => x.Id.ToString() == id).Include(x => x.FeatureTranslations).SingleOrDefaultAsync();
			_context.Features.Remove(feature);
			await _context.SaveChangesAsync();
		}
		#endregion


		#region CRUD Academic Subjects
		public async Task<List<AcademicSubject>> GetAcademicSubjects()
		{
			var academicSubjects = await _context.AcademicSubjects.Include(x => x.AcademicSubjectTranslations).AsNoTracking().ToListAsync();

			foreach (var academicSubject in academicSubjects)
			{
				if (academicSubject.AcademicSubjectTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (academicSubject.AcademicSubjectTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								academicSubject.AcademicSubjectTranslations.Add(new AcademicSubjectTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								academicSubject.AcademicSubjectTranslations.Add(new AcademicSubjectTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return academicSubjects;
		}

		public async Task CreateAcademicSubject(AcademicSubjectEditModel model)
		{
			var academicSubject = new AcademicSubject()
			{
				AcademicSubjectTranslations = new Collection<AcademicSubjectTranslation>()
			};

			for (var i = 0; i < _cultures.Count; i++)
			{
				academicSubject.AcademicSubjectTranslations.Add(new AcademicSubjectTranslation
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.AcademicSubjects.AddAsync(academicSubject);
			await _context.SaveChangesAsync();
			return;
		}

		public async Task EditAcademicSubject(String id, AcademicSubjectEditModel model)
		{
			var academicSubject = await _context.AcademicSubjects.Where(x => x.Id.ToString() == id).Include(x => x.AcademicSubjectTranslations).SingleOrDefaultAsync();

			if (academicSubject == null)
			{
				return;
			}

			academicSubject.AcademicSubjectTranslations.Clear();

			for (var i = 0; i < model.Language.Count; i++)
			{
				academicSubject.AcademicSubjectTranslations.Add(new AcademicSubjectTranslation()
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();

			return;
		}

		public async Task DeleteAcademicSubject(String id)
		{
			var academicSubject = await _context.AcademicSubjects.Where(x => x.Id.ToString() == id).Include(x => x.AcademicSubjectTranslations).SingleOrDefaultAsync();
			_context.AcademicSubjects.Remove(academicSubject);
			await _context.SaveChangesAsync();
		}
		#endregion


		#region CRUD Humans
		public async Task<List<Human>> GetTeachers()
		{
			var teachers = await _context.Humans.Where(x => x.Post == "Teacher").Include(x => x.HumanTranslations).Include(x => x.Picture).AsNoTracking().ToListAsync();

			foreach (var teacher in teachers)
			{
				if (teacher.HumanTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (teacher.HumanTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								teacher.HumanTranslations.Add(new HumanTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								teacher.HumanTranslations.Add(new HumanTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return teachers;
		}

		public async Task<List<Human>> GetGraduates()
		{
			var grasuates = await _context.Humans.Where(x => x.Post == "Graduate").Include(x => x.HumanTranslations).Include(x => x.Picture).AsNoTracking().ToListAsync();

			foreach (var graduate in grasuates)
			{
				if (graduate.HumanTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (graduate.HumanTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								graduate.HumanTranslations.Add(new HumanTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								graduate.HumanTranslations.Add(new HumanTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return grasuates;
		}

		public async Task CreateHuman(HumanCreateModel model)
		{
			var human = new Human()
			{
				Post = model.Post,
				HumanTranslations = new Collection<HumanTranslation>()
			};

			for (var i = 0; i < _cultures.Count; i++)
			{
				human.HumanTranslations.Add(new HumanTranslation
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			human.Picture = await createPicture(model.Picture, "img/teachers");
			human.PictureId = human.Picture.Id;

			await _context.Humans.AddAsync(human);
			await _context.SaveChangesAsync();
			return;
		}

		public async Task EditHuman(String id, HumanEditModel model)
		{
			var human = await _context.Humans.Where(x => x.Id.ToString() == id).Include(x => x.HumanTranslations).Include(x => x.Picture).SingleOrDefaultAsync();

			if (human == null)
			{
				return;
			}

			if (model.Picture != null)
			{
				var oldPicture = human.Picture; // Track to delete it after changing, cause human.picture cannot be null in db
				human.Picture = await createPicture(model.Picture, "img/teachers"); // getting new picture from model
				await deletePictureAsync(oldPicture.Id.ToString(), "img/teachers");
				_context.Pictures.Remove(oldPicture); // Deleting tracked old picture
			}

			human.HumanTranslations.Clear();
			for (var i = 0; i < model.Language.Count; i++)
			{
				human.HumanTranslations.Add(new HumanTranslation()
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();
			return;
		}

		public async Task DeleteHuman(String id)
		{
			var human = await _context.Humans.Where(x => x.Id.ToString() == id).Include(x => x.Picture).SingleOrDefaultAsync();
			_context.Humans.Remove(human);
			await deletePictureAsync(human.Picture.Id.ToString(), "img/teachers");
			await _context.SaveChangesAsync();
		}

		#endregion


		#region C**D Pictures
		private async Task<Picture> createPicture(IFormFile modelPicture, String localPath)
		{
			Picture newPicture = new Picture()
			{
				Id = Guid.NewGuid(),
				Created = DateTime.UtcNow
			};

			var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(modelPicture.ContentDisposition).FileName.Value.Trim('"'));
			var fileExt = Path.GetExtension(fileName);

			var attachmentPath = Path.Combine(_hostingEnvironment.WebRootPath, localPath, newPicture.Id.ToString("N") + fileExt);
			newPicture.Path = $"/{localPath}/{newPicture.Id:N}{fileExt}";

			using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
			{
				await modelPicture.CopyToAsync(fileStream);
			}

			await _context.Pictures.AddAsync(newPicture);
			return newPicture;
		}

		private async Task deletePictureAsync(String id, String localPath)
		{
			var picture = await _context.Pictures.SingleOrDefaultAsync(x => x.Id.ToString() == id);
			File.Delete(Path.Combine(_hostingEnvironment.WebRootPath, localPath, picture.Id.ToString("N") + Path.GetExtension(picture.Path)));
			var dbInstance = await _context.Pictures.SingleOrDefaultAsync(x => x.Id == picture.Id);
			_context.Pictures.Remove(dbInstance);
		}
		#endregion


		#region CRUD CityFeatures
		public async Task<List<CityFeature>> GetCityFeatures()
		{
			var cityFeatures = await _context.CityFeatures.Include(x => x.CityFeatureTranslations).Include(x => x.Pictures).Where(x => x.Pictures.Count == 0).AsNoTracking().ToListAsync();

			foreach (var cityFeature in cityFeatures)
			{
				if (cityFeature.CityFeatureTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (cityFeature.CityFeatureTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								cityFeature.CityFeatureTranslations.Add(new CityFeatureTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								cityFeature.CityFeatureTranslations.Add(new CityFeatureTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaultDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return cityFeatures;

		}

		public async Task<CityFeature> GetCityFeatureWithPhotos()
		{
			return await _context.CityFeatures.Include(x => x.CityFeatureTranslations).Include(x => x.Pictures)
											   .Where(x => x.Pictures.Count != 0).AsNoTracking().SingleOrDefaultAsync();
		}

		public async Task CreateCityFeature(CityFeatureEditModel model)
		{
			var cityFeature = new CityFeature()
			{
				CityFeatureTranslations = new Collection<CityFeatureTranslation>()
			};

			if (model.Pictures != null)
			{
				cityFeature.Pictures = new List<Picture>();
				foreach (var pictureToAdd in model.Pictures)
				{
					var pic = await createPicture(pictureToAdd, "img/city_features");
					pic.CityFeatureId = cityFeature.Id;
					cityFeature.Pictures.Add(pic);
				}
			}

			for (var i = 0; i < _cultures.Count; i++)
			{
				cityFeature.CityFeatureTranslations.Add(new CityFeatureTranslation
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.CityFeatures.AddAsync(cityFeature);
			await _context.SaveChangesAsync();
			return;
		}

		public async Task EditCityFeature(String id, CityFeatureEditModel model)
		{
			var cityFeature = await _context.CityFeatures.Where(x => x.Id.ToString() == id).Include(x => x.CityFeatureTranslations).Include(x => x.Pictures).SingleOrDefaultAsync();

			if (cityFeature == null)
			{
				return;
			}

			if (model.PicturesToDelete != null)
			{
				foreach (var pictureTodelete in model.PicturesToDelete)
				{
					await deletePictureAsync(pictureTodelete.ToString(), "img/city_features");
				}
			}

			if (model.Pictures != null)
			{
				foreach (var pictureToAdd in model.Pictures)
				{
					var pic = await createPicture(pictureToAdd, "img/city_features");
					pic.CityFeatureId = cityFeature.Id;
					cityFeature.Pictures.Add(pic);
				}
			}

			cityFeature.CityFeatureTranslations.Clear();

			for (var i = 0; i < model.Language.Count; i++)
			{
				cityFeature.CityFeatureTranslations.Add(new CityFeatureTranslation()
				{
					Name = model.Name[i],
					Description = model.Description != null ? model.Description[i] : null,
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();

			return;
		}

		public async Task DeleteCityFeature(String id)
		{
			var cityFeature = await _context.CityFeatures.Where(x => x.Id.ToString() == id).Include(x => x.CityFeatureTranslations).SingleOrDefaultAsync();
			_context.CityFeatures.Remove(cityFeature);
			await _context.SaveChangesAsync();
		}

		#endregion


		#region CRUD Dormitory
		public async Task<List<Dormitory>> GetDormitories()
		{
			var dormitories = await _context.Dormitories.Include(x => x.DormitoryTranslations).Include(x => x.Pictures).AsNoTracking().ToListAsync();

			foreach (var cityFeature in dormitories)
			{
				if (cityFeature.DormitoryTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
				{
					foreach (var culture in _cultures)
					{
						if (cityFeature.DormitoryTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
						{
							try // .resx contains requiered values
							{
								cityFeature.DormitoryTranslations.Add(new DormitoryTranslation()
								{
									Name = _resourceManager.GetString("DefaultName", culture),
									Description = _resourceManager.GetString("DefaultDescription", culture),
									Language = culture.Name.ToString(),
								});
							}
							catch // .resx file wasn't updated yet.
							{
								cityFeature.DormitoryTranslations.Add(new DormitoryTranslation()
								{
									Name = _localizer.GetString("DefaultName"),
									Description = _localizer.GetString("DefaulDescription"),
									Language = culture.Name.ToString(),
								});
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return dormitories;
		}

		public async Task CreateDormitory(DormitoryCreateModel model)
		{
			var dormitory = new Dormitory()
			{
				DormitoryTranslations = new Collection<DormitoryTranslation>(),
				Pictures = new Collection<Picture>()
			};

			foreach (var picture in model.Pictures)
			{
				dormitory.Pictures.Add(await createPicture(picture, "img/dormitories"));
			}

			for (var i = 0; i < _cultures.Count; i++)
			{
				dormitory.DormitoryTranslations.Add(new DormitoryTranslation
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.Dormitories.AddAsync(dormitory);
			await _context.SaveChangesAsync();
			return;
		}
		public async Task EditDormitory(String id, DormitoryEditModel model)
		{
			var dormitory = await _context.Dormitories.Where(x => x.Id.ToString() == id).Include(x => x.DormitoryTranslations).Include(x => x.Pictures).SingleOrDefaultAsync();

			if (dormitory == null)
			{
				return;
			}

			if (model.PicturesToDelete != null)
			{
				foreach (var pictureTodelete in model.PicturesToDelete)
				{
					await deletePictureAsync(pictureTodelete.ToString(), "img/dormitories");
				}
			}

			if (model.Pictures != null)
			{
				foreach (var pictureToAdd in model.Pictures)
				{
					var pic = await createPicture(pictureToAdd, "img/dormitories");
					pic.DormitoryId = dormitory.Id;
					dormitory.Pictures.Add(pic);
				}
			}

			dormitory.DormitoryTranslations.Clear();
			for (var i = 0; i < model.Language.Count; i++)
			{
				dormitory.DormitoryTranslations.Add(new DormitoryTranslation()
				{
					Name = model.Name[i],
					Description = model.Description[i],
					Language = model.Language[i]
				});
			}

			await _context.SaveChangesAsync();
			return;
		}
		public async Task DeleteDormitory(String id)
		{
			var dormitory = await _context.Dormitories.Where(x => x.Id.ToString() == id).Include(x => x.Pictures).SingleOrDefaultAsync();
			_context.Dormitories.Remove(dormitory);
			foreach (var pic in dormitory.Pictures)
			{
				await deletePictureAsync(pic.Id.ToString(), "img/dormitories");
			}
			await _context.SaveChangesAsync();
		}
		#endregion


		#region CRU* Condition
		public async Task<Condition> GetCondition()
		{
			var condition = await _context.Conditions.Include(x => x.ConditionTranslations).SingleOrDefaultAsync();

			if (condition == null)
			{
				condition = new Condition()
				{
					ConditionTranslations = new Collection<ConditionTranslation>()
				};
				_context.Conditions.Add(condition);
			}

			if (condition.ConditionTranslations.Count < _cultures.Count) // if some language doesn't have a translation for entity
			{
				foreach (var culture in _cultures)
				{
					if (condition.ConditionTranslations.Where(x => x.Language == culture.Name.ToString()).SingleOrDefault() == default) // selecting empty translations
					{
						try // .resx contains requiered values
						{
							condition.ConditionTranslations.Add(new ConditionTranslation()
							{
								Description = _resourceManager.GetString("DefaultDescription", culture),
								Language = culture.Name.ToString(),
							});
						}
						catch // .resx file wasn't updated yet.
						{
							condition.ConditionTranslations.Add(new ConditionTranslation()
							{
								Description = _localizer.GetString("DefaulDescription"),
								Language = culture.Name.ToString(),
							});
						}
					}
				}
			}

			await _context.SaveChangesAsync();
			return condition;
		}

		public async Task<Condition> EditCondition(ConditionsEditModel model)
		{
			var condition = await _context.Conditions.Include(x => x.ConditionTranslations).SingleOrDefaultAsync();

			if (condition == null)
			{
				return null;
			}


			for (var i = 0; i < model.Language.Count; i++)
			{
				condition.ConditionTranslations.ElementAt(i).Description = model.Description[i];
				condition.ConditionTranslations.ElementAt(i).Language = model.Language[i];
			}

			await _context.SaveChangesAsync();

			return condition;
		}
		#endregion


		#region JSON
		public async Task<Dictionary<String, String>> GetBlockName(String projectBlockName = default)
		{
			if (projectBlockName != default)
			{
				NameOfPageBlockModel block = default;

				JsonSerializer serializer = new JsonSerializer();
				using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
				using StreamReader streamReader = new StreamReader(fileStreamer);
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					while (jsonReader.Read())
					{
						// deserialize only when there's "[" character in the stream
						if (jsonReader.TokenType == JsonToken.StartArray)
						{
							block = serializer.Deserialize<List<NameOfPageBlockModel>>(jsonReader)
								.Where(x => x.ProjectName == projectBlockName)
								.SingleOrDefault();
						}
					}
				}

				if (block == default || block.Translations.Count < _cultures.Count)
				{
					block = await createDefaultBlockName(projectBlockName, block);
				}

				return block.Translations;
			}

			return default;
		}
		public async void ChangeBlockName(String projectBlockName, MainPageBlockEditModel model)
		{
			var blockNames = getBlockNames();  // to reserialize all of them

			if (blockNames.Count > 0)  // can't change any name if no one block exists
			{
				var newBlockName = blockNames.Where(x => x.ProjectName == projectBlockName).FirstOrDefault();

				newBlockName.Translations.Clear();
				for (var i = 0; i < _cultures.Count; i++)
				{
					newBlockName.Translations.Add(model.Language[i], model.NewBlockName[i]);
				}

				using FileStream fileStreamer = new FileStream("NameOfPageBlock.json", FileMode.Create);
				await System.Text.Json.JsonSerializer.SerializeAsync(fileStreamer, blockNames);
			}
		}

		#region HiddenLogic
		private async Task<NameOfPageBlockModel> createDefaultBlockName(String projectBlockName, NameOfPageBlockModel oldBlock)
		{
			var blockNames = getBlockNames();  // to reserialize all of them


			var defaultBlock = new NameOfPageBlockModel()
			{
				ProjectName = projectBlockName,
				Translations = new Dictionary<string, string>()
			};

			if (oldBlock != default)  // If some translations was into oldBlock early
			{
				blockNames.Remove(blockNames.Where(x => x.ProjectName == oldBlock.ProjectName).SingleOrDefault()); // .Remove(oldBlock) is don't work... Idk, why?
				foreach (var oldTranslation in oldBlock.Translations)
				{
					defaultBlock.Translations.Add(oldTranslation.Key, oldTranslation.Value);
				}
			}

			foreach (var culture in _cultures)
			{
				if (!defaultBlock.Translations.TryGetValue(culture.Name, out var smthVar))
				{
					try
					{
						defaultBlock.Translations.Add(culture.Name.ToString(), _resourceManager.GetString("DefaultName", culture));
					}
					catch // if we need to implement a new language but .resx file wasn't updated yet.
					{
						defaultBlock.Translations.Add(culture.Name.ToString(), _localizer.GetString("DefaultName"));
					}
				}
			}

			blockNames.Add(defaultBlock);

			using FileStream fileStreamer = new FileStream("NameOfPageBlock.json", FileMode.Create);
			await System.Text.Json.JsonSerializer.SerializeAsync(fileStreamer, blockNames);

			return defaultBlock;
		}
		private List<NameOfPageBlockModel> getBlockNames()
		{
			List<NameOfPageBlockModel> blocks = new List<NameOfPageBlockModel>();

			JsonSerializer serializer = new JsonSerializer();
			using FileStream fileStreamer = File.Open("NameOfPageBlock.json", FileMode.Open);
			using StreamReader streamReader = new StreamReader(fileStreamer);
			using JsonReader jsonReader = new JsonTextReader(streamReader);

			while (jsonReader.Read())
			{
				// deserialize only when there's "[" character in the stream
				if (jsonReader.TokenType == JsonToken.StartArray)
				{
					blocks = serializer.Deserialize<List<NameOfPageBlockModel>>(jsonReader);
				}
			}

			return blocks;
		}
		#endregion

		#endregion
	}
}
