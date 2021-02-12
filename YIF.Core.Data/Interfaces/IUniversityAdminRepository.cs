using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
namespace YIF.Core.Data.Interfaces
{
    public interface IUniversityAdminRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniAdmin(UniversityAdmin universityAdmin);
        Task<string> Delete(string adminId);
        Task<K> GetById(string id);
        Task<K> GetByUniversityId(string universityId);
        Task<K> GetByUniversityIdWithoutIsDeletedCheck(string universityId);
        Task<IEnumerable<K>> GetAllUniAdmins();
    }
}
