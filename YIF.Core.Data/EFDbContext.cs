using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data
{
    public class EFDbContext : IdentityDbContext<DbUser, IdentityRole, string, IdentityUserClaim<string>,
    IdentityUserRole<string>, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {       
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        #region Tables
        public DbSet<UniversityModerator> UniversityModerators { get; set; }
        public DbSet<UniversityAdmin> UniversityAdmins { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Speciality> Specialities { get; set; }

        public DbSet<SchoolModerator> SchoolModerators { get; set; }
        public DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        public DbSet<Graduate> Graduates { get; set; }
        public DbSet<School> Schools { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        { 
            base.OnModelCreating(builder);

            

            #region University

            //builder.Entity<UniversityModerator>()
            //    .HasOne(x => x.Admin)
            //    .WithOne(x => x.Moderator)
            //    .OnDelete(DeleteBehavior.Cascade);

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

            builder.Entity<University>()
                .HasMany(x => x.Admins)
                .WithOne(x => x.University)
                .HasForeignKey(x => x.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<University>()
                .HasMany(x => x.Lectures)
                .WithOne(x => x.University)
                .HasForeignKey(x => x.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<SpecialityToUniversity>()
                .HasKey(c => new { c.Id,c.UniversityId, c.SpecialityId });                                        

            //builder.Entity<University>()
            //.HasMany(x => x.Specialities)
            //.WithMany(x => x.Universities);

            #endregion

            #region School

            //builder.Entity<SchoolModerator>()
            //    .HasOne(x => x.Admin)
            //    .WithOne(x => x.Moderator)
            //    .OnDelete(DeleteBehavior.Cascade);

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
