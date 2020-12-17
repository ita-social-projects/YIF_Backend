using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServicesInterfaces;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using System.Threading.Tasks;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ViewModels;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels.UserViewModels;

namespace YIF.Core.Service.Concrete.Services
{
    public class UserService : IUserService<DbUser>
    {
        private readonly IRepository<DbUser, UserDTO> _userRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        public UserService(IRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager, 
            IJwtService _IJwtService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> GetAllUsers()
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                return result.Set(false, $"There are not users in database");
            }

            var mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<UserDTO, UserViewModel>();
            }));
            
            result.Object = mapper.Map<IEnumerable<UserViewModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> GetUserById(string id)
        {
            var result = new ResponseModel<UserViewModel>();
            var user = await _userRepository.Get(id);
            if (user == null)
            {
                return result.Set(false, $"User not found:  {id}");
            }
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()));
            
            result.Object = mapper.Map<UserViewModel>(user);
            return result.Set(true);
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> FindUser(Expression<Func<DbUser, bool>> predicate)
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<UserDTO, UserViewModel>();
            }));

            result.Object = mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.Find(predicate));
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> CreateUser(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<LoginResponseViewModel>> LoginUser(LoginViewModel loginModel)
        {
            var result = new ResponseModel<LoginResponseViewModel>();
            result.Object = new LoginResponseViewModel();

            var user = await _userManager.FindByEmailAsync(loginModel.email);
            if(user == null)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.password, false, false);
            if(!loginResult.Succeeded)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var token = _jwtService.CreateTokenByUser(user);
            await _signInManager.SignInAsync(user, isPersistent: false);

            result.Object.userToken = token;

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
