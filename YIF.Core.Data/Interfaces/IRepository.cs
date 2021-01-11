using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface IRepository<T, K> : IDisposable
        where T : class
        where K : class
    {
        Task<string> Create(T dbUser, object entityUser, string userPassword, string role);
        Task<bool> Update(T item);
        Task<bool> Delete(string id);
        Task<K> Get(string id);
        Task<K> GetByEmail(string email);
        Task<IEnumerable<K>> GetAll();
        Task<IEnumerable<K>> Find(Expression<Func<T, bool>> predicate);
        Task<bool> UpdateUserToken(DbUser user, string refreshToken);
        Task<bool> UpdateUserPhoto(DbUser user, string photo);

    }
}
