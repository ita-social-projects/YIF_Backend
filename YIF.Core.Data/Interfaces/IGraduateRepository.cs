using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IGraduateRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<K> GetByUserId(string userId);
    }
}
