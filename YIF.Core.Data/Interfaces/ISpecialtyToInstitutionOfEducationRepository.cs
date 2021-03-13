using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToInstitutionOfEducationRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<IEnumerable<K>> GetSpecialtyInInstitutionOfEducationDescriptionsById(string id);
        Task AddFavorite(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate);
        Task RemoveFavorite(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate);
        Task<bool> FavoriteContains(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate);
    }
}