using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IUniversityModeratorRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniModerator(UniversityModerator universityModerator);
        Task<bool> Delete(string id);
        Task<K> GetById(string id);
        Task<IEnumerable<K>> GetAllUniModerators();
    }
}
