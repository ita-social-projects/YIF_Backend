using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByName(string name);
        Task<bool> Exist(string schoolName);

        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsStrings();
    }
}
