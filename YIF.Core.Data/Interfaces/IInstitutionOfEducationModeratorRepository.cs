using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationModeratorRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<string> AddUniModerator(T institutionOfEducationModerator);
        Task<IEnumerable<K>> GetByIoEId(string ioEId);
        Task<K> GetByUserId(string userId);
        Task<K> GetModeratorForAdmin(string id, string adminId);
        Task<K> GetByAdminId(string id, string adminId);
        Task<string> Disable(T ioEModerator);
        Task<string> Enable(T ioEModerator);
    }
}
