using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;

namespace YIF.Core.Domain.ServicesInterfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ResponseModel<UserViewModel>> GetUserById(string id);
        Task<ResponseModel<IEnumerable<UserViewModel>>> GetAllUsers();
        Task<ResponseModel<IEnumerable<UserViewModel>>> FindUser(Expression<Func<T, bool>> predicate);

        Task<ResponseModel<UserViewModel>> CreateUser(UserDTO userDTO);
        Task<ResponseModel<LoginResponseViewModel>> LoginUser(LoginDTO loginDTO);
        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(int? id);

        void Dispose();
    }
}
