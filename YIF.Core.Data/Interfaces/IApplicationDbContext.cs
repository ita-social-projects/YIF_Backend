using Microsoft.EntityFrameworkCore;
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
        DbSet<Speciality> Specialities { get; set; }
        DbSet<DirectionToUniversity> DirectionsToUniversities { get; set; }

        DbSet<SchoolModerator> SchoolModerators { get; set; }
        DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        DbSet<Graduate> Graduates { get; set; }
        DbSet<School> Schools { get; set; }
        DbSet<DbUser> Users { get; set; }

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
