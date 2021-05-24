using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolAdminRepository<TEntity>
        where TEntity : class
    {
        Task<string> AddSchoolAdmin(TEntity schoolAdmin);
        Task<TEntity> GetBySchoolId(string schoolId);
        Task<string> Delete(string adminId);
        Task<TEntity> GetBySchoolIdWithoutIsDeletedCheck(string schoolId);
        Task<IEnumerable<TEntity>> GetAllSchoolAdmins();
    }
}
