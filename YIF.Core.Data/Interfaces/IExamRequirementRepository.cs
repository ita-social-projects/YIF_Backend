using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IExamRequirementRepository <T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task AddRange(params T[] items);
        Task DeleteRangeByDescriptionId(string id);
    }
}
