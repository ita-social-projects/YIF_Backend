using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationAdminRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<string> AddUniAdmin(T institutionOfEducationAdmin);
        Task<string> Delete(DbUser user);
        Task<T> Disable(T adminId);
        Task<T> Enable(T adminId);
        Task<K> GetByInstitutionOfEducationId(string institutionOfEducationId);
        Task<K> GetByInstitutionOfEducationIdWithoutIsDeletedCheck(string institutionOfEducationId);
        Task<K> GetUserByAdminId(string id);
        Task<IEnumerable<K>> GetAllUniAdmins();
        Task<K> GetByUserId(string userId);
    }
}
