using System.Threading.Tasks;

namespace YIF.Core.Data.Interfaces
{
    public interface ITokenRepository<T>
        where T : class
    {
        Task<bool> AddUserToken(T token);
        Task<T> FindUserToken(string userId);
        Task<bool> UpdateUserToken(string userId, string refreshToken);
    }
}
