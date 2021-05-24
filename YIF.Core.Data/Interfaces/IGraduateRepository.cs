using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IGraduateRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByUserId(string userId);
    }
}
