namespace YIF.Core.Data.Interfaces
{
    public interface IPaymentFormRepository <T, K>: IRepository<T, K>
        where T: class
        where K: class
    {
    }
}
