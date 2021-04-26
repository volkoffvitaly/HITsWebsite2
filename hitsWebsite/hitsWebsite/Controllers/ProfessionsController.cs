using hitsWebsite.Data;
using hitsWebsite.Models;
using hitsWebsite.Models.ViewModels;
using hitsWebsite.Services;
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

        public ProfessionsController(IDataProviderService dataProviderService)
        {
            this._dataProviderService = dataProviderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Professions/AddProfession")]
        [HttpPost]
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
    }
}
