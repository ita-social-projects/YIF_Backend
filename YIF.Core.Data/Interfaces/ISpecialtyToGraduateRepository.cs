using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToGraduateRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task AddFavorite(TEntity specialtyToGraduate);
        Task RemoveFavorite(TEntity specialtyToGraduate);
    }
}
