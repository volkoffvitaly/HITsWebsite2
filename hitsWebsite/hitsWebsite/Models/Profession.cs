using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Profession
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
