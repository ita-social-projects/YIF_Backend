using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationModeratorRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<K> GetByUserId(string id);
        Task<string> AddUniModerator(T institutionOfEducationModerator);
        Task<IEnumerable<K>> GetByIoEId(string ioEId);
    }
}
