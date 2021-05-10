using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using hitsWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class HomeController : Controller
    {
        private readonly IDataProviderService _dataProviderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IDataProviderService dataProviderService, UserManager<ApplicationUser> userManager)
        {
            _dataProviderService = dataProviderService;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CultureManagement(LanguageEditModel model, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(model.Culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDynamicPageInfo(String projectNameOfPage, DynamicPageEditModel model, String currentControllerName = null)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(projectNameOfPage))
            {
                await _dataProviderService.ChangeDynamicPageInfo(projectNameOfPage, model);
            }

            return RedirectToLocal(currentControllerName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlockName(String projectBlockName, MainPageBlockEditModel model, String currentControllerName = null)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(projectBlockName))
            {
                await _dataProviderService.ChangeBlockName(projectBlockName, model);
            }

            return RedirectToLocal(currentControllerName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRequest(RequestCreateModel model, String currentControllerName = null)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.CreateRequest(model);
            }

            return RedirectToLocal(currentControllerName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFooter(FooterEditModel model, String currentControllerName = null)
        {
            if (ModelState.IsValid)
            {
                await _dataProviderService.EditFooter(model);
            }

            return RedirectToLocal(currentControllerName);
        }

        private IActionResult RedirectToLocal(String controllerName)
        {
            if (controllerName == null)
            {
                return this.RedirectToAction("Index", "Professions");
            }
            else
            {
                return this.RedirectToAction("Index", $"{controllerName}");
            }
        }
    }
}
