using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class CityFeatureTranslation
    {
        public Guid Id { get; set; }

        [Required]
        public String Language { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }


        public CityFeature CityFeature { get; set; }
        public Guid CityFeatureId { get; set; }
    }
}
