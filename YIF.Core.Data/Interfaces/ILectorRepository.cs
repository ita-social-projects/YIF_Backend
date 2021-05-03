using System;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ILectorRepository<T, K> : IDisposable
        where T : class
        where K : class
    {
        Task Add(T lector);
    }
}
