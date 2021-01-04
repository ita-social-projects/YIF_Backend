using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IUniversityRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniversity(University university);
        Task<K> GetByName(string name);
        Task<IEnumerable<K>> GetAllUniversities();
    }
}
