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
        public DbSet<EducationForm> EducationForms { get; set; }
        public DbSet<PaymentForm> PaymentForms { get; set; }
        public  DbSet<SpecialtyInUniversityDescription> SpecialtyInUniversityDescriptions { get; set; }
        public DbSet<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }
        public DbSet<EducationFormToDescription> EducationFormToDescriptions { get; set; }
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
                .HasOne(x => x.SpecialtyInUniversityDescription)
                .WithMany(x => x.SpecialtyToUniversities)
                .HasForeignKey(x => x.SpecialtyInUniversityDescriptionId);
            
            builder.Entity<PaymentFormToDescription>()
                .HasKey(k => new {k.Id, k.PaymentFormId, k.SpecialtyInUniversityDescriptionId});

            builder.Entity<PaymentFormToDescription>()
                .HasOne(x => x.SpecialtyInUniversityDescription)
                .WithMany(x => x.PaymentFormToDescriptions)
                .HasForeignKey(k => k.SpecialtyInUniversityDescriptionId);

            builder.Entity<PaymentFormToDescription>()
                .HasOne(x => x.PaymentForm)
                .WithMany(x => x.PaymentFormToDescriptions)
                .HasForeignKey(k => k.PaymentFormId);

            builder.Entity<EducationFormToDescription>()
                .HasKey(k => new {k.Id, k.EducationFormId, k.SpecialtyInUniversityDescriptionId});

            builder.Entity<EducationFormToDescription>()
                .HasOne(x => x.EducationForm)
                .WithMany(x => x.EducationFormToDescriptions)
                .HasForeignKey(k => k.EducationFormId);

            builder.Entity<EducationFormToDescription>()
                .HasOne(x => x.SpecialtyInUniversityDescription)
                .WithMany(x => x.EducationFormToDescriptions)
                .HasForeignKey(k => k.SpecialtyInUniversityDescriptionId);

            builder.Entity<ExamRequirement>()
                .HasKey(k => new {k.Id, k.ExamId, k.SpecialtyInUniversityDescriptionId});

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.Exam)
                .WithMany(x => x.ExamRequirements)
                .HasForeignKey(x => x.ExamId);

            builder.Entity<ExamRequirement>()
                .HasOne(x => x.SpecialtyInUniversityDescription)
                .WithMany(x => x.ExamRequirements)
                .HasForeignKey(x => x.SpecialtyInUniversityDescriptionId);

            builder.Entity<SpecialtyToGraduate>()
                .HasKey(c => new { c.Id, c.GraduateId, c.SpecialtyId });

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
