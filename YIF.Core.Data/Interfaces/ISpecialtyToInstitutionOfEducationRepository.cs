using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToInstitutionOfEducationRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetSpecialtyToIoEDescriptionsById(string id);
        Task AddSpecialty(TEntity specialtyToInstitutionOfEducation);
    }
}
