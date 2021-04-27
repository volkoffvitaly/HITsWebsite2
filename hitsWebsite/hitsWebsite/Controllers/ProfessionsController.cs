using hitsWebsite.Data;
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
    public class ProfessionsController : Controller
    {
        private readonly IDataProviderService _dataProviderService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfessionsController(IDataProviderService dataProviderService, UserManager<ApplicationUser> userManager)
        {
            this._dataProviderService = dataProviderService;
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> AddProfession(ProfessionEditModel model)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.CreateProfession(model);
                return RedirectToAction("Index");
            }

            ViewBag.ProfessionModel = model;
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public IActionResult EditBlockName(String projectBlockName, MainPageBlockEditModel model)
        {
            if (ModelState.IsValid)
            {
                _dataProviderService.ChangeBlockName(projectBlockName, model);
            }

            ViewBag.ChangeBlockModel = model;
            return RedirectToAction("Index");
        }
    }
}
