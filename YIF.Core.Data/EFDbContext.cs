using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
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
        public DbSet<InstitutionOfEducationModerator> InstitutionOfEducationModerators { get; set; }
        public DbSet<InstitutionOfEducationAdmin> InstitutionOfEducationAdmins { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<InstitutionOfEducation> InstitutionOfEducations { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DirectionToInstitutionOfEducation> DirectionsToInstitutionOfEducations { get; set; }
        public DbSet<SpecialtyToInstitutionOfEducation> SpecialtyToInstitutionOfEducations { get; set; }
        public DbSet<InstitutionOfEducationToGraduate> InstitutionOfEducationsToGraduates { get; set; }
        public DbSet<SpecialtyToGraduate> SpecialtyToGraduates { get; set; }
        public DbSet<SchoolModerator> SchoolModerators { get; set; }
        public DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        public DbSet<Graduate> Graduates { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamRequirement> ExamRequirements { get; set; }
        public DbSet<SpecialtyToIoEDescription> SpecialtyToIoEDescriptions { get; set; }
        public DbSet<SpecialtyToInstitutionOfEducationToGraduate> SpecialtyToInstitutionOfEducationToGraduates { get; set; }

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

            #region User

            builder.Entity<SuperAdmin>()
                .HasOne(x => x.User)
                .WithMany(x => x.SuperAdmins)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
            

            builder.Entity<BaseUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.BaseUsers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Graduate>()
                .HasOne(x => x.User)
                .WithMany(x => x.Graduates)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
         
            builder.Entity<InstitutionOfEducationAdmin>()
                .HasOne(x => x.User)
                .WithMany(x => x.InstitutionOfEducationAdmins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Lecture>()
                .HasOne(x => x.User)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchoolModerator>()
                .HasOne(x => x.User)
                .WithMany(x => x.SchoolModerators)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Token>()
                .HasOne(x => x.User)
                .WithOne(x => x.Token)
                .HasForeignKey<Token>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.UserProfile)
                .HasForeignKey<UserProfile>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region InstitutionOfEducation

            builder.Entity<Lecture>()
                .HasOne(x => x.InstitutionOfEducation)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.InstitutionOfEducationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InstitutionOfEducationAdmin>()
                .HasOne(x => x.InstitutionOfEducation)
                .WithMany(x => x.Admins)
                .HasForeignKey(x => x.InstitutionOfEducationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InstitutionOfEducationModerator>()
                .HasOne(x => x.Admin)
                .WithMany(x => x.Moderators)
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InstitutionOfEducationToGraduate>()
                .HasKey(ug => new {ug.InstitutionOfEducationId, ug.GraduateId});

            builder.Entity<InstitutionOfEducationToGraduate>()
                .HasOne(ug => ug.InstitutionOfEducation)
                .WithMany(u => u.InstitutionOfEducationGraduates)
                .HasForeignKey(ug => ug.InstitutionOfEducationId);

            builder.Entity<InstitutionOfEducationToGraduate>()
                .HasOne(ug => ug.Graduate)
                .WithMany(g => g.InstitutionOfEducationGraduates)
                .HasForeignKey(ug => ug.GraduateId);

            builder.Entity<InstitutionOfEducation>()
                .Property(e => e.InstitutionOfEducationType)
                .HasConversion(
                    v => v.ToString(),
                    v => (InstitutionOfEducationType)Enum.Parse(typeof(InstitutionOfEducationType), v));

            builder.Entity<Direction>()
                .HasMany(x => x.Specialties)
                .WithOne(x => x.Direction)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DirectionToInstitutionOfEducation>()
                .HasKey(c => new {c.Id, c.InstitutionOfEducationId, c.DirectionId});

            builder.Entity<DirectionToInstitutionOfEducation>()
                .HasOne(x => x.Direction)
                .WithMany(x => x.DirectionToInstitutionOfEducations)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Specialty>()
                .HasOne(x => x.Direction)
                .WithMany(x => x.Specialties)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SpecialtyToInstitutionOfEducation>()
                .HasOne(x => x.Specialty)
                .WithMany(x => x.SpecialtyToInstitutionOfEducations)
                .HasForeignKey(x => x.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SpecialtyToInstitutionOfEducation>()
                .HasOne(x => x.InstitutionOfEducation)
                .WithMany(x => x.SpecialtyToInstitutionOfEducations)
                .HasForeignKey(x => x.InstitutionOfEducationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SpecialtyToIoEDescription>()
                .Property(e => e.EducationForm)
                .HasConversion(
                v => v.ToString(),
                v => (EducationForm)Enum.Parse(typeof(EducationForm), v));

            builder.Entity<SpecialtyToIoEDescription>()
                .Property(e => e.PaymentForm)
                .HasConversion(
                v => v.ToString(),
                v => (PaymentForm)Enum.Parse(typeof(PaymentForm), v));

            builder.Entity<SpecialtyToIoEDescription>()
                .HasOne(x => x.SpecialtyToInstitutionOfEducation)
                .WithMany(x => x.SpecialtyToIoEDescriptions)
                .HasForeignKey(x => x.SpecialtyToInstitutionOfEducationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DirectionToInstitutionOfEducation>()
                .HasOne(x => x.InstitutionOfEducation)
                .WithMany(x => x.DirectionToInstitutionOfEducation)
                .HasForeignKey(x => x.InstitutionOfEducationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ExamRequirement>()
                .HasKey(k => new {k.Id, k.ExamId, k.SpecialtyToIoEDescriptionId});

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.Exam)
                .WithMany(x => x.ExamRequirements)
                .HasForeignKey(x => x.ExamId);

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.SpecialtyToIoEDescription)
                .WithMany(x => x.ExamRequirements)
                .HasForeignKey(x => x.SpecialtyToIoEDescriptionId);
            
            builder.Entity<SpecialtyToGraduate>()
                .HasKey(c => new { c.GraduateId, c.SpecialtyId });

            builder.Entity<SpecialtyToGraduate>()
                .HasOne(sg => sg.Specialty)
                .WithMany(u => u.SpecialtyToGraduates)
                .HasForeignKey(sg => sg.SpecialtyId);

            builder.Entity<SpecialtyToGraduate>()
                .HasOne(sg => sg.Graduate)
                .WithMany(g => g.SpecialtyToGraduates)
                .HasForeignKey(sg => sg.GraduateId);

            builder.Entity<SpecialtyToInstitutionOfEducationToGraduate>()
                .HasKey(k => new { k.SpecialtyId, k.InstitutionOfEducationId, k.GraduateId });

            builder.Entity<SpecialtyToInstitutionOfEducationToGraduate>()
                .HasOne(x => x.Specialty)
                .WithMany(x => x.SpecialtyToInstitutionOfEducationToGraduates)
                .HasForeignKey(x => x.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SpecialtyToInstitutionOfEducationToGraduate>()
               .HasOne(x => x.InstitutionOfEducation)
               .WithMany(x => x.SpecialtyToInstitutionOfEducationToGraduates)
               .HasForeignKey(x => x.InstitutionOfEducationId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SpecialtyToInstitutionOfEducationToGraduate>()
               .HasOne(x => x.Graduate)
               .WithMany(x => x.SpecialtyToInstitutionOfEducationToGraduates)
               .HasForeignKey(x => x.GraduateId)
               .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region School

            builder.Entity<SchoolModerator>()
                .HasOne(x => x.Admin)
                .WithMany(x => x.SchoolModerators)
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SchoolModerator>()
                .HasOne(x => x.School)
                .WithMany(x => x.Moderators)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Graduate>()
                .HasOne(x => x.School)
                .WithMany(x => x.Graduates)
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
