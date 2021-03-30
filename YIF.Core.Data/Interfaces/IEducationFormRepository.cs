namespace YIF.Core.Data.Interfaces
{
    public interface IEducationFormRepository <T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
    }
}
