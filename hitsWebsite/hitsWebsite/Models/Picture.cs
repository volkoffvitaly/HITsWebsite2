using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace hitsWebsite.Models
{
    public class Picture
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public String Path { get; set; }
    }
}
