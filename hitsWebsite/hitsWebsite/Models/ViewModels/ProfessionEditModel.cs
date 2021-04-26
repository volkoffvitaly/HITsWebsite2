using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hitsWebsite.Models.ViewModels
{
    public class ProfessionEditModel
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String Description { get; set; }
    }
}
