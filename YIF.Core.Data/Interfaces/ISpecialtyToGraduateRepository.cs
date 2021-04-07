using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToGraduateRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task AddFavorite(T specialtyToGraduate);
        Task RemoveFavorite(T specialtyToGraduate);
    }
}
