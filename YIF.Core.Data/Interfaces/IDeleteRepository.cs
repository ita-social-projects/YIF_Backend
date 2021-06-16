using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IDeleteRepository
    {
        Task<string> Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}
