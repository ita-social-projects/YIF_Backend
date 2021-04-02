using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ISpecialtyToIoEDescriptionRepository <T, K> : IRepository<T, K>
        where T: class
        where K: class
    {
        Task<K> GetEducationForms();
        Task<K> GetPaymentForms();
        Task<string> Add(T specialtyToIoEDescription);
        Task<bool> Contains(string Id);

    }
}
