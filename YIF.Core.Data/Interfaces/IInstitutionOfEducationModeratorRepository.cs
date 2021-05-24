using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationModeratorRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<string> AddUniModerator(TEntity institutionOfEducationModerator);
        Task<IEnumerable<TEntity>> GetByIoEId(string ioEId);
        Task<TEntity> GetByUserId(string userId);
        Task<TEntity> GetModeratorForAdmin(string id, string adminId);
        Task<TEntity> GetByAdminId(string id, string adminId);
        Task<string> Disable(TEntity ioEModerator);
        Task<string> Enable(TEntity ioEModerator);
    }
}
