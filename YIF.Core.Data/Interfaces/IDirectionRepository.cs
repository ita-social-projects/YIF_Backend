using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IDirectionRepository <T, K> : IRepository<T, K>
        where T: class
        where K: class
    {
        Task<IEnumerable<K>> GetByIoEId(string InstitytionOfEducationId);
        Task Add(T direction);
    }
}
