using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Human
    {
        public Guid Id { get; set; }
        public String Post { get; set; }
        public ICollection<HumanTranslation> HumanTranslations { get; set; }

        public Picture Picture { get; set; }
        public Guid PictruId { get; set; }
    }
}
