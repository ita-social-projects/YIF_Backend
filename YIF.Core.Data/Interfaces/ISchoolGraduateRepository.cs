using System;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolGraduateRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetSchoolByUserId(string userId);
    }
}
