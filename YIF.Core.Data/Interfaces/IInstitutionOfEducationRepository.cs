using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<K> AddInstitutionOfEducation(InstitutionOfEducation institutionOfEducation);
        Task<K> GetByName(string name);
        Task AddFavorite(InstitutionOfEducationToGraduate institutionOfEducationToGraduate);
        Task RemoveFavorite(InstitutionOfEducationToGraduate institutionOfEducationToGraduate);
        Task<IEnumerable<K>> GetFavoritesByUserId(string userId);
        Task<bool> ContainsById(string id);
    }
}
