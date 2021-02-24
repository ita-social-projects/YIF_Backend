using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<SuperAdmin> SuperAdmins { get; set; }
        DbSet<UniversityModerator> UniversityModerators { get; set; }
        DbSet<UniversityAdmin> UniversityAdmins { get; set; }
        DbSet<Lecture> Lectures { get; set; }
        DbSet<University> Universities { get; set; }
        DbSet<Direction> Directions { get; set; }
        DbSet<Specialty> Specialties { get; set; }
        DbSet<DirectionToUniversity> DirectionsToUniversities { get; set; }
        DbSet<SpecialtyToUniversity> SpecialtyToUniversities { get; set; }
        DbSet<SpecialtyToGraduate> SpecialtyToGraduates { get; set; }
        DbSet<SchoolModerator> SchoolModerators { get; set; }
        DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        DbSet<Graduate> Graduates { get; set; }
        DbSet<School> Schools { get; set; }
        DbSet<DbUser> Users { get; set; }
        DbSet<Token> Tokens { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<UniversityToGraduate> UniversitiesToGraduates { get; set; }
        DbSet<Exam> Exams { get; set; }
        DbSet<ExamRequirement> ExamRequirements { get; set; }
        DbSet<EducationForm> EducationForms { get; set; }
        DbSet<PaymentForm> PaymentForms { get; set; }
        DbSet<SpecialtyInUniversityDescription> SpecialtyInUniversityDescriptions { get; set; }
        DbSet<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }
        DbSet<EducationFormToDescription> EducationFormToDescriptions { get; set; }

        Task<int> SaveChangesAsync();
        int SaveChanges();

        ValueTask<EntityEntry<T>> AddAsync<T>(T entity) where T : class;
    }
}
