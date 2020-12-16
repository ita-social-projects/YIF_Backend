using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IRepository<T, K> : IDisposable
        where T : class
        where K : class
    {
        Task<K> Create(T item);
        Task<bool> Update(T item);
        Task<bool> Delete(string id);
        Task<K> Get(string id);
        Task<IEnumerable<K>> GetAll();
        Task<IEnumerable<K>> Find(Expression<Func<T, bool>> predicate);
    }
}
