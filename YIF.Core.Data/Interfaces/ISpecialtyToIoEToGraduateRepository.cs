using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToIoEToGraduateRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task AddFavorite(T specialtyToInstitutionOfEducationToGraduate);
        Task RemoveFavorite(T specialtyToInstitutionOfEducationToGraduate);
        Task<bool> FavoriteContains(T specialtyToInstitutionOfEducationToGraduate);
    }
}
