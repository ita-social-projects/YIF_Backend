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
using YIF.Core.Domain.ApiModels;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.UserApiModels;
using YIF.Core.Data.Entities;

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

        public async Task<ResponseModel<IEnumerable<UserApiModel>>> GetAllUsers()
        {
            var result = new ResponseModel<IEnumerable<UserApiModel>>();
            var users = (List<UserDTO>)await _userRepository.GetAll();
            if (users.Count < 1)
            {
                return result.Set(false, $"There are not users in database");
            }
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseModel<UserApiModel>> GetUserById(string id)
        {
            var result = new ResponseModel<UserApiModel>();
            try
            {
                var user = await _userRepository.Get(id);
                result.Object = _mapper.Map<UserApiModel>(user);
            }
            catch (KeyNotFoundException ex)
            {
                return result.Set(false, ex.Message);
            }
            return result.Set(true);
        }

        public async Task<ResponseModel<IEnumerable<UserApiModel>>> FindUser(Expression<Func<DbUser, bool>> predicate)
        {
            var result = new ResponseModel<IEnumerable<UserApiModel>>();
            var foundUsers = await _userRepository.Find(predicate);
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(foundUsers);
            return result.Set(true);
        }

        public async Task<ResponseModel<LoginResultApiModel>> RegisterUser(RegisterViewModel registerModel)
        {
            var result = new ResponseModel<LoginResultApiModel>();
            //result.Object = string.Empty;

            var searchUser = _userManager.FindByEmailAsync(registerModel.Email);
            if(searchUser.Result != null)
            {
                return result.Set(false, "User already exist");
            }

            if(!registerModel.Password.Equals(registerModel.ConfirmPassword))
            {
                return result.Set(false, "Password and confirm password does not compare");
            }

            var dbUser = new DbUser 
            { 
                Email = registerModel.Email,
                UserName = registerModel.Username
            };
            var graduate = new Graduate();
            var registerResult = await _userRepository.Create(dbUser, graduate, registerModel.Password);

            if (registerResult != string.Empty)
            {
                return result.Set(false, registerResult);
            }

            var token = _jwtService.CreateTokenByUser(dbUser);
            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            result.Object = new LoginResultApiModel { Token = token };

            return result.Set(true);
        }

        public async Task<ResponseModel<LoginResultApiModel>> LoginUser(LoginViewModel loginModel)
        {
            var result = new ResponseModel<LoginResultApiModel>();

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
            
            result.Object = new LoginResultApiModel() { Token = token };

            return result.Set(true);
        }

        public async Task<bool> UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserById(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
