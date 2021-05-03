using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Document
    {
        public Guid Id { get; set; } 
        public ICollection<DocumentTranslation> DocumentTranslations { get; set; }
    }
}
