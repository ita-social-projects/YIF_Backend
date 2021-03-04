using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyRepository<T, K>: IRepository<T, K>
        where T : class
        where K : class
    {
        Task<bool> ContainsById(string id);
    }
}
