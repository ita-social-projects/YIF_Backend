﻿using System;
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

        Task<ResponseModel<LoginResponseViewModel>> RegisterUser(RegisterViewModel registerModel);
        Task<ResponseModel<LoginResponseViewModel>> LoginUser(LoginViewModel loginModel);
        Task<bool> UpdateUser(UserDTO user);
        Task<bool> DeleteUserById(int? id);

        void Dispose();
    }
}
