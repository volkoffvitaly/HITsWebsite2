﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HITs_Website_2._0.Controllers
{
    public class ProfessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}