using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToIoEToGraduateRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task AddFavorite(TEntity specialtyToInstitutionOfEducationToGraduate);
        Task RemoveFavorite(TEntity specialtyToInstitutionOfEducationToGraduate);
        Task<bool> FavoriteContains(TEntity specialtyToInstitutionOfEducationToGraduate);
    }
}
