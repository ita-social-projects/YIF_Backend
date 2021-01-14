﻿using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IUserRepository<T, K> : IRepository<T, K>
        where T : class
        where K : class
    {
        Task<string> Create(T dbUser, object entityUser, string userPassword, string role);
        Task<K> GetByEmail(string email);
        Task<T> GetUserWithToken(string userId);
        Task<T> GetUserWithUserProfile(string userId);
        Task SetDefaultUserProfileIfEmpty(string userId);
        Task<bool> SetUserProfile(UserProfile profile, string userId);
        Task<bool> UpdateUserToken(T user, string refreshToken);
        Task<bool> UpdateUserPhoto(T user, string photo);
    }
}
