using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyRepository<T, K>: IRepository<T, K>
        where T : class
        where K : class
    {
        Task<bool> ContainsById(string id); 
        Task AddFavorite(SpecialtyToGraduate specialtyToGraduate);
        Task RemoveFavorite(SpecialtyToGraduate specialtyToGraduate);
        Task<IEnumerable<K>> GetFavoritesByUserId(string userId);
        Task Add(T specialty);
    }
}
