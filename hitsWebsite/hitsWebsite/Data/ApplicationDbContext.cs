using hitsWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace hitsWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            //
        }


        public DbSet<ProfessionModel> Professions { get; set; }
        public DbSet<ProfessionTranslationModel> ProfessionTranslations { get; set; }

        public DbSet<FeatureModel> Features { get; set; }
        public DbSet<FeatureTranslationModel> FeatureTranslations { get; set; }

        public DbSet<DynamicPageModel> DynamicPages { get; set; }
        public DbSet<DynamicPageTranslationModel> DynamicPageTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
