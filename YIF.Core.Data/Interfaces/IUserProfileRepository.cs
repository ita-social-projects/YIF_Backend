using System;
using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    /// <summary>
    /// The interface of epository for the work with database user.
    /// </summary>
    /// <typeparam name="L">User profile entity.</typeparam>
    /// <typeparam name="M">User profile data transfer object.</typeparam>
    public interface IUserProfileRepository<L, M> : IDisposable
        where L : class
        where M : class
    {
        Task<L> GetDefaultUserProfile(string userId);
        Task<L> SetDefaultUserProfileIfEmpty(string userId);
        Task<M> SetUserProfile(M profile, string schoolName = null);
    }
}
