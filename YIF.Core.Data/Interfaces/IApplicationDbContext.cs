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
        DbSet<InstitutionOfEducationModerator> InstitutionOfEducationModerators { get; set; }
        DbSet<InstitutionOfEducationAdmin> InstitutionOfEducationAdmins { get; set; }
        DbSet<Lector> Lectors { get; set; }
        DbSet<InstitutionOfEducation> InstitutionOfEducations { get; set; }
        DbSet<Direction> Directions { get; set; }
        DbSet<Specialty> Specialties { get; set; }
        DbSet<DirectionToInstitutionOfEducation> DirectionsToInstitutionOfEducations { get; set; }
        DbSet<SpecialtyToInstitutionOfEducation> SpecialtyToInstitutionOfEducations { get; set; }
        DbSet<SpecialtyToGraduate> SpecialtyToGraduates { get; set; }
        DbSet<SchoolModerator> SchoolModerators { get; set; }
        DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        DbSet<Graduate> Graduates { get; set; }
        DbSet<School> Schools { get; set; }
        DbSet<DbUser> Users { get; set; }
        DbSet<Token> Tokens { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<InstitutionOfEducationToGraduate> InstitutionOfEducationsToGraduates { get; set; }
        DbSet<Exam> Exams { get; set; }
        DbSet<ExamRequirement> ExamRequirements { get; set; }
        DbSet<SpecialtyToIoEDescription> SpecialtyToIoEDescriptions { get; set; }
        DbSet<SpecialtyToInstitutionOfEducationToGraduate> SpecialtyToInstitutionOfEducationToGraduates { get; set; }


        Task<int> SaveChangesAsync();
        int SaveChanges();

        ValueTask<EntityEntry<T>> AddAsync<T>(T entity) where T : class;
        EntityEntry<T> GetEntry<T>(T entity) where T : class;
    }
}
