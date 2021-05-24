using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task Update(TEntity item);
        Task Delete(string id);
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
