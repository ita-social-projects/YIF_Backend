using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF.Core.Domain.ViewModels.UserViewModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ResponseModel<UserViewModel>> GetUserById(string id);
        Task<ResponseModel<IEnumerable<UserViewModel>>> GetAllUsers();
        Task<ResponseModel<IEnumerable<UserViewModel>>> FindUser(Expression<Func<T, bool>> predicate);

        Task<ResponseModel<AuthenticateResponseVM>> LoginUser(LoginViewModel loginModel);
        Task<ResponseModel<AuthenticateResponseVM>> RegisterUser(RegisterViewModel registerModel);
        Task<ResponseModel<AuthenticateResponseVM>> RefreshToken(TokenRequestApiModel tokenApiModel);

        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(string id);

        void Dispose();
    }
}
