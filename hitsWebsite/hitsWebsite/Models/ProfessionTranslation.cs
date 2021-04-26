using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class ProfessionTranslation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Language { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public Guid Profession_Id { get; set; }
    }
}
