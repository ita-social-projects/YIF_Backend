using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResultApiModels;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ResponseApiModel<UserApiModel>> GetUserById(string id);
        Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAllUsers();
        Task<ResponseApiModel<IEnumerable<UserApiModel>>> FindUser(Expression<Func<T, bool>> predicate);

        Task<ResponseApiModel<LoginResultApiModel>> LoginUser(LoginApiModel loginModel);
        Task<ResponseApiModel<LoginResultApiModel>> RegisterUser(RegisterApiModel registerModel);
        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(string id);

        void Dispose();
    }
}
