using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class FooterTranslation
    {
        public Guid Id { get; set; }

        [Required]
        public String Language { get; set; } 
        [Required]
        public String Description { get; set; }


        public Footer Footer { get; set; }
        public Guid FooterId { get; set; }
    }
}
