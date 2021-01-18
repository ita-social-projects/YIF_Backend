using System;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolGraduateRepository<T> : IDisposable where T : class
    {
        Task<T> GetSchoolByUserId(string userId);
    }
}
