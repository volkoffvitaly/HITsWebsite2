using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Controllers
{
    public class LiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
