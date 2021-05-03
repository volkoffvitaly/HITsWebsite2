using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Dormitory
    {
        public Guid Id { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<DormitoryTranslation> DormitoryTranslations { get; set; }
    }
}
