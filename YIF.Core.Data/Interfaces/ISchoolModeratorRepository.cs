using System;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolModeratorRepository<TEntity>
        where TEntity : class
    {
        Task<string> AddSchoolModerator(TEntity schoolModerator);
    }
}
