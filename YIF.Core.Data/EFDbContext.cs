﻿using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Data
{
    public class EFDbContext : IdentityDbContext<DbUser, IdentityRole, string, IdentityUserClaim<string>,
    IdentityUserRole<string>, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext
    {       
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        #region Tables
        public DbSet<SuperAdmin> SuperAdmins { get; set; }

        public DbSet<UniversityModerator> UniversityModerators { get; set; }
        public DbSet<UniversityAdmin> UniversityAdmins { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DirectionToUniversity> DirectionsToUniversities { get; set; }
        public DbSet<SpecialtyToUniversity> SpecialtyToUniversities { get; set; }
        public DbSet<UniversityToGraduate> UniversitiesToGraduates { get; set; }

        public DbSet<SchoolModerator> SchoolModerators { get; set; }
        public DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        public DbSet<Graduate> Graduates { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamRequirement> ExamToSpecialtyToUniversities { get; set; }
        public DbSet<EducationForm> EducationForms { get; set; }
        public DbSet<PaymentForm> PaymentForms { get; set; }
        public  DbSet<SpecialtyInUniversityDescription> SpecialtyInUniversityDescriptions { get; set; }
        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async ValueTask<EntityEntry<T>> AddAsync<T>(T entity) where T : class
        {      
            return await base.AddAsync(entity);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SuperAdmin>()
                .HasOne(x => x.User);

            #region University

            builder.Entity<UniversityModerator>()
                .HasOne(x => x.Admin)
                .WithOne(x => x.Moderator)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UniversityModerator>()
                .HasOne(x => x.University)
                .WithMany(x => x.Moderators)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Lecture>()
                .HasOne(x => x.University)
                .WithMany(x => x.Lectures)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<University>()
                .HasMany(x => x.Moderators)
                .WithOne(x => x.University)
                .HasForeignKey(x => x.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UniversityToGraduate>()
                .HasKey(ug => new {ug.UniversityId, ug.GraduateId});

            builder.Entity<UniversityToGraduate>()
                .HasOne(ug => ug.University)
                .WithMany(u => u.UniversityGraduates)
                .HasForeignKey(ug => ug.UniversityId);

            builder.Entity<UniversityToGraduate>()
                .HasOne(ug => ug.Graduate)
                .WithMany(g => g.UniversityGraduates)
                .HasForeignKey(ug => ug.GraduateId);

            //builder.Entity<University>()//потестить каскадку
            //    .HasMany(x => x.Admins)
            //    .WithOne(x => x.University)
            //    .HasForeignKey(x => x.UniversityId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<University>()
                .HasMany(x => x.Lectures)
                .WithOne(x => x.University)
                .HasForeignKey(x => x.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Direction>()
                .HasMany(x => x.Specialties)
                .WithOne(x => x.Direction)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DirectionToUniversity>()
                .HasKey(c => new {c.Id, c.UniversityId, c.DirectionId});

            builder.Entity<SpecialtyToUniversity>()
                .HasKey(c => new {c.Id, c.UniversityId, c.SpecialtyId});

            builder.Entity<SpecialtyToUniversity>()
                .HasOne(x => x.SpecialtyInUniversityDescription);

            builder.Entity<SpecialtyInUniversityDescription>()
                .HasOne(x => x.PaymentForm);

            builder.Entity<SpecialtyInUniversityDescription>()
                .HasOne(x => x.EducationForm);

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.Exam);


            builder.Entity<ExamRequirement>()
                .HasOne(er => er.Exam);

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.SpecialtyInUniversityDescription)
                .WithMany(x => x.ExamRequirements)
                .HasForeignKey(x => x.SpecialtyInUniversityDescriptionId);

            //builder.Entity<SpecialtyToUniversity>()
            //    .HasOne(x=>x.EducationForm);

            //builder.Entity<SpecialtyToUniversity>()
            //    .HasMany(x => x.ExamRequirements)
            //    .WithOne(x => x.SpecialtyToUniversity)
            //    .HasForeignKey(x => x.SpecialtyToUniversityId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<ExamRequirement>()
            //    .HasOne(x => x.Exam);

            #endregion

            #region School

            builder.Entity<SchoolModerator>()
                .HasOne(x => x.Admin)
                .WithOne(x => x.Moderator)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchoolModerator>()
                .HasOne(x => x.School)
                .WithMany(x => x.Moderators)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Graduate>()
                .HasOne(x => x.School)
                .WithMany(x => x.Graduates)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
