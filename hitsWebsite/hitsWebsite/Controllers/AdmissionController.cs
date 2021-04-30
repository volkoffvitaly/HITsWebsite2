using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using hitsWebsite.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Controllers
{
    public class AdmissionController : Controller
    {
        private readonly IDataProviderService _dataProviderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdmissionController(IDataProviderService dataProviderService, UserManager<ApplicationUser> userManager)
        {
            _dataProviderService = dataProviderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAcademicSubject(AcademicSubjectEditModel model)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.CreateAcademicSubject(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAcademicSubject(String id, AcademicSubjectEditModel model)
        {
            if (ModelState.IsValid && id != null)
            {
                await _dataProviderService.EditAcademicSubject(id, model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAcademicSubject(String id)
        {
            if (id != null)
            {
                await _dataProviderService.DeleteAcademicSubject(id);
            }

            return RedirectToAction("Index");
        }
    }
}
