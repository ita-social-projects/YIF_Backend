using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF.Core.Domain.ViewModels.UserViewModels;

namespace YIF.Core.Service.Concrete.Services
{
    public class UserService : IUserService<DbUser>
    {
        private readonly IRepository<DbUser, UserDTO> _userRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public UserService(IRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager, 
            IJwtService _IJwtService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> GetAllUsers()
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                return result.Set(false, $"There are not users in database");
            }
            result.Object = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> GetUserById(string id)
        {
            var result = new ResponseModel<UserViewModel>();
            try
            {
                var user = await _userRepository.Get(id);
                result.Object = _mapper.Map<UserViewModel>(user);
            }
            catch (KeyNotFoundException ex)
            {
                return result.Set(false, ex.Message);
            }
            return result.Set(true);
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> FindUser(Expression<Func<DbUser, bool>> predicate)
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            result.Object = _mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.Find(predicate));
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> CreateUser(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> LoginUser(LoginViewModel loginModel)
        {
            var result = new ResponseModel<string>();
            //result.Object = new LoginResponseViewModel();

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if(user == null)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if(!loginResult.Succeeded)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var token = _jwtService.CreateTokenByUser(user);
            await _signInManager.SignInAsync(user, isPersistent: false);

            result.Object = token;

            return result.Set(true);
        }


        public async Task<bool> DeleteUserById(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
