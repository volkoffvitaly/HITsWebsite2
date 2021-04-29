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
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class ProfessionsController : Controller
    {
        private readonly IDataProviderService _dataProviderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessionsController(IDataProviderService dataProviderService, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> EditDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model)
        {
            if (ModelState.IsValid && projectNameOfPage != null)
            {
                await _dataProviderService.ChangeDynamicPageInfo(projectNameOfPage, model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfession(ProfessionEditModel model)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.CreateProfession(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfession(String id, ProfessionEditModel model)
        {
            if (ModelState.IsValid && id != null)
            {
                await _dataProviderService.EditProfession(id, model);
            }

            return RedirectToAction("Index");
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfession(String id)
        {
            if (id != null)
            {
                await _dataProviderService.DeleteProfession(id);
            }
 
            return RedirectToAction("Index");
        }
    }
}
