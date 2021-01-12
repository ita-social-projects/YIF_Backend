using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyRepository<T> : IDisposable
        where T : class
    {
        Task<T> GetById(string name);
        Task<T> GetByName(string name);
        Task<IEnumerable<T>> GetAllSpecialties();
    }
}
