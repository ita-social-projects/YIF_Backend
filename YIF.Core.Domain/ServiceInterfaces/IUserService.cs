using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ApiModels;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.UserApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ResponseModel<UserApiModel>> GetUserById(string id);
        Task<ResponseModel<IEnumerable<UserApiModel>>> GetAllUsers();
        Task<ResponseModel<IEnumerable<UserApiModel>>> FindUser(Expression<Func<T, bool>> predicate);

        Task<ResponseModel<LoginResultApiModel>> LoginUser(LoginViewModel loginModel);
        Task<ResponseModel<LoginResultApiModel>> RegisterUser(RegisterViewModel registerModel);
        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(string id);

        void Dispose();
    }
}
