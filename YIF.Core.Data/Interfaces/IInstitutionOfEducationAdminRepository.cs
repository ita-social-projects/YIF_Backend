using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationAdminRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<string> AddUniAdmin(TEntity institutionOfEducationAdmin);
        Task<string> Delete(DbUser user);
        Task<string> Disable(TEntity adminId);
        Task<string> Enable(TEntity adminId);
        Task<TEntity> GetByInstitutionOfEducationId(string institutionOfEducationId);
        Task<TEntity> GetByInstitutionOfEducationIdWithoutIsDeletedCheck(string institutionOfEducationId);
        Task<TEntity> GetUserByAdminId(string id);
        Task<IEnumerable<TEntity>> GetAllUniAdmins();
        Task<TEntity> GetByUserId(string userId);
    }
}
