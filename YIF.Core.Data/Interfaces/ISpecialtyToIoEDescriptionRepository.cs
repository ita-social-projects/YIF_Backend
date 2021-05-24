using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToIoEDescriptionRepository <TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task Add(TEntity specialtyToIoEDescription);
    }
}
