using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationModeratorRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniModerator(InstitutionOfEducationModerator institutionOfEducationModerator);
        Task<bool> Delete(string id);
        Task<K> GetByUserId(string id);
        Task<IEnumerable<K>> GetAllUniModerators();
    }
}
