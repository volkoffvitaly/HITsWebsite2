using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Condition
    {
        public Guid Id { get; set; } 
        public ICollection<ConditionTranslation> ConditionTranslations { get; set; }
 
    }
}
