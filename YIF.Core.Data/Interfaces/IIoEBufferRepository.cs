using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IIoEBufferRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task Add(T IoEBuffer);
    }
}
