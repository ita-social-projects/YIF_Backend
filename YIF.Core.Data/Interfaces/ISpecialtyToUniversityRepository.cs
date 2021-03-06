using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToUniversityRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<IEnumerable<K>> GetSpecialtyInUniversityDescriptionsById(string id);
        Task AddFavorite(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate);
        Task RemoveFavorite(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate);
        Task<bool> FavoriteContains(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate);
    }
}