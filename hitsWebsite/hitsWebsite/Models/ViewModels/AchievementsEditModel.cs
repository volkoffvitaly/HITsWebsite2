using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Models.ViewModels
{
    public class AchievementsEditModel
    {
        public List<String> Description { get; set; }
        public List<String> Language { get; set; }
    }
}
