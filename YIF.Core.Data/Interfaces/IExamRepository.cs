namespace YIF.Core.Data.Interfaces
{
    public interface IExamRepository <TEntity> : IRepository<TEntity>
        where TEntity: class
    {
    }
}
