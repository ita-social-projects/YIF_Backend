using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface IUserRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<string> Create(TEntity dbUser, object entityUser, string userPassword, string role);
        Task<TEntity> GetByEmail(string email);
        Task<TEntity> GetUserWithToken(string userId);
        Task<TEntity> GetUserWithUserProfile(string userId);
        Task<bool> UpdateUserPhoto(TEntity user, string photo);
        Task<bool> Exist(string userId);
        Task<TEntity> GetUserWithRoles(string userId);
    }
}
