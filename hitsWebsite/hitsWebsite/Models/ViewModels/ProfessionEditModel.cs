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
        public List<String> Name { get; set; }

        [Required]
        public List<String> Description { get; set; }

        [Required]
        public List<String> Language { get; set; }
    }
}
