using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ILectorRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task Add(T lector);
        Task<K> GetLectorByUserAndIoEIds(string userId, string ioEId);
        Task<IEnumerable<K>> GetLectorsByIoEId(string ioEId);
    }
}
