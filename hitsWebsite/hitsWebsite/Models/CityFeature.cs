using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class CityFeature
    {
        public Guid Id { get; set; }
        public List<Picture> Pictures { get; set; }
        public ICollection<CityFeatureTranslation> CityFeatureTranslations { get; set; }
    }
}
