using HITs_Website_2._0.Data;
using HITs_Website_2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HITs_Website_2._0.Controllers
{
    public class ProfessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Professions/AddProfession")]
        [HttpPost]
        public async Task<IActionResult> AddProfession()
        {
            var profession = new Profession
            {
                Name = "Data scientist",
                Description = "Some desc"
            };

            _context.Professions.Add(profession);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
