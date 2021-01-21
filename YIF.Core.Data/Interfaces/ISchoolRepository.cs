using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolRepository<K> : IDisposable
        where K : class
    {
        Task<K> GetByName(string name);

        Task<IEnumerable<K>> GetAll();
    }
}
