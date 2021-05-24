using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IDirectionRepository <TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetByIoEId(string institutionOfEducationId);
    }
}
