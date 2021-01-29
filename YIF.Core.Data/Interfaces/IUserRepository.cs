using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    /// <summary>
    /// The interface of repository for the work with database user.
    /// </summary>
    /// <typeparam name="T">User entity.</typeparam>
    /// <typeparam name="K">User data transfer object.</typeparam>
    public interface IUserRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<string> Create(T dbUser, object entityUser, string userPassword, string role);
        Task<K> GetByEmail(string email);
        Task<T> GetUserWithToken(string userId);
        Task<K> GetUserWithUserProfile(string userId);
        Task<bool> UpdateUserPhoto(K user, string photo);
    }
}
