using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    /// <summary>
    /// The interface of epository for the work with database user.
    /// </summary>
    /// <typeparam name="T">User entity.</typeparam>
    /// <typeparam name="K">User data transfer object.</typeparam>
    /// <typeparam name="L">User profile entity.</typeparam>
    /// <typeparam name="M">User profile data transfer object.</typeparam>
    public interface IUserRepository<T, K, L, M> : IRepository<T, K>
        where T : class
        where K : class
        where L : class
        where M : class
    {
        Task<string> Create(T dbUser, object entityUser, string userPassword, string role);
        Task<K> GetByEmail(string email);
        Task<T> GetUserWithToken(string userId);
        Task<K> GetUserWithUserProfile(string userId);
        Task<L> GetDefaultUserProfile(string userId);
        Task<L> SetDefaultUserProfileIfEmpty(string userId);
        Task<M> SetUserProfile(M profile, string schoolName = null);
        Task<bool> UpdateUserPhoto(K user, string photo);
    }
}
