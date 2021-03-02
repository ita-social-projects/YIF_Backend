using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToUniversityRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<IEnumerable<K>> GetSpecialtyInUniversityDescriptionsById(string id);
    }
}