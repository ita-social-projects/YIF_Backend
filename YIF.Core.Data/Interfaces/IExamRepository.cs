namespace YIF.Core.Data.Interfaces
{
    public interface IExamRepository <T, K> : IRepository<T, K>
        where T: class
        where K: class
    {
    }
}
