using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class DynamicPageTranslation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public String Language { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }


        public DynamicPage DynamicPage { get; set; }
        public Guid DynamicPageId { get; set; }
    }
}
