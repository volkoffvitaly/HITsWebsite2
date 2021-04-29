using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class DynamicPage
    {
        public Guid Id { get; set; }

        [Required]
        public String ProjectName { get; set; }

        public ICollection<DynamicPageTranslation> DynamicPageTranslations { get; set; } = new Collection<DynamicPageTranslation>();
    }
}
