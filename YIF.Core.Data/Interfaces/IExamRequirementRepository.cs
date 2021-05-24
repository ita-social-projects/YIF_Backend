using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IExamRequirementRepository <TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task DeleteRangeByDescriptionId(string id);
    }
}
