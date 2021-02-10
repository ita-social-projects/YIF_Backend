using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolAdminRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddSchoolAdmin(SchoolAdmin schoolAdmin);
        Task<K> GetBySchoolId(string schoolId);
        Task<string> Delete(string adminId);
        Task<K> GetBySchoolIdWithoutIsDeletedCheck(string schoolId);
        Task<IEnumerable<K>> GetAllSchoolAdmins();
    }
}
