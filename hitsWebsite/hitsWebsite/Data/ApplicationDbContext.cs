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

        public DbSet<CityFeature> CityFeatures { get; set; }
        public DbSet<CityFeatureTranslation> CityFeatureTranslations { get; set; }

        public DbSet<Dormitory> Dormitories { get; set; }
        public DbSet<DormitoryTranslation> DormitoryTranslations { get; set; }

        public DbSet<AcademicSubject> AcademicSubjects { get; set; }
        public DbSet<AcademicSubjectTranslation> AcademicSubjectTranslations { get; set; }

        public DbSet<DynamicPage> DynamicPages { get; set; }
        public DbSet<DynamicPageTranslation> DynamicPageTranslations { get; set; }

        public DbSet<Human> Humans { get; set; }
        public DbSet<HumanTranslation> HumanTranslations { get; set; }

        public DbSet<Condition> Conditions { get; set; }
        public DbSet<ConditionTranslation> ConditionTranslations { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentTranslation> DocumentTranslations { get; set; }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AchievementTranslation> AchievementTranslations { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
