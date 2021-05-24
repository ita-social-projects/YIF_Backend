using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<bool> ContainsById(string id);
        Task<IEnumerable<TEntity>> GetFavoritesByUserId(string userId);
        Task Add(TEntity specialty);
    }
}
