using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Feature
    {
        public Guid Id { get; set; }
        public ICollection<FeatureTranslation> FeatureTranslations { get; set; }
    }
}
