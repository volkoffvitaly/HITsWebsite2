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


        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionTranslation> ProfessionTranslations { get; set; }

        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureTranslation> FeatureTranslations { get; set; }

        public DbSet<AcademicSubject> AcademicSubjects { get; set; }
        public DbSet<AcademicSubjectTranslation> AcademicSubjectTranslations { get; set; }

        public DbSet<DynamicPage> DynamicPages { get; set; }
        public DbSet<DynamicPageTranslation> DynamicPageTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
