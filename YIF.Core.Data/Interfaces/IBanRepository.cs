using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IBanRepository
        
    {
        Task<string> Ban<TEntity>(TEntity entity) where TEntity : class;
        Task<string> Unban<TEntity>(TEntity entity) where TEntity : class;
    }
}
