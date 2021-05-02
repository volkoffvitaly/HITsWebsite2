using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using hitsWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class LiveController : Controller
    {
        private readonly IDataProviderService _dataProviderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LiveController(IDataProviderService dataProviderService, UserManager<ApplicationUser> userManager)
        {
            _dataProviderService = dataProviderService;
            _userManager = userManager;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCityFeature(CityFeatureEditModel model)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.CreateCityFeature(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCityFeature(String id, CityFeatureEditModel model)
        {
            if (ModelState.IsValid && id != null)
            {
                await _dataProviderService.EditCityFeature(id, model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityFeature(String id)
        {
            if (id != null)
            {
                await _dataProviderService.DeleteCityFeature(id);
            }

            return RedirectToAction("Index");
        }
    }
}
