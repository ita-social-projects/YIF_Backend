using System.Collections.Generic;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToInstitutionOfEducationRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<IEnumerable<K>> GetSpecialtyToIoEDescriptionsById(string id);
        Task AddSpecialty(T specialtyToInstitutionOfEducation);
        Task AddRange(IEnumerable<T> collectionOfSpecialties);
        Task<K> GetById(string id);
    }
}
