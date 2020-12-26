using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
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
            var users = (List<UserDTO>)await _userRepository.GetAll();
            if (users.Count < 1)
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
            var foundUsers = await _userRepository.Find(predicate);
            result.Object = _mapper.Map<IEnumerable<UserViewModel>>(foundUsers);
            return result.Set(true);
        }

        public async Task<ResponseModel<AuthenticateResponseVM>> RegisterUser(RegisterViewModel registerModel)
        {
            var result = new ResponseModel<AuthenticateResponseVM>();
            //result.Object = string.Empty;

            var searchUser = _userManager.FindByEmailAsync(registerModel.Email);
            if (searchUser.Result != null)
            {
                return result.Set(false, "User already exist");
            }

            if (!registerModel.Password.Equals(registerModel.ConfirmPassword))
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

            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _userRepository.UpdateUserToken(dbUser, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            result.Object = new AuthenticateResponseVM { Token = token, RefreshToken = refreshToken };

            return result.Set(true);
        }

        public async Task<ResponseModel<AuthenticateResponseVM>> LoginUser(LoginViewModel loginModel)
        {
            var result = new ResponseModel<AuthenticateResponseVM>();

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (!loginResult.Succeeded)
            {
                return result.Set(false, "Login or password is incorrect");
            }

            var token = _jwtService.CreateToken(_jwtService.SetClaims(user));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _userRepository.UpdateUserToken(user, refreshToken);

            await _signInManager.SignInAsync(user, isPersistent: false);

            result.Object = new AuthenticateResponseVM() { Token = token, RefreshToken = refreshToken };

            return result.Set(true);
        }

        public async Task<ResponseModel<AuthenticateResponseVM>> RefreshToken(TokenRequestApiModel tokenApiModel)
        {
            var result = new ResponseModel<AuthenticateResponseVM>();

            string accessToken = tokenApiModel.Token;
            string refreshToken = tokenApiModel.RefreshToken;

            var claims = _jwtService.GetClaimsFromExpiredToken(accessToken);
            if (claims == null)
            {
                return result.Set(false, "Invalid client request");
            }
            var userId = claims.First(claim => claim.Type == "id").Value;

            var user = await _userManager.Users.Include(u => u.Token).SingleAsync(x => x.Id == userId);

            if (user == null || user.Token == null || user.Token.RefreshToken != refreshToken || user.Token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return result.Set(false, "Invalid client request");
            }

            var newAccessToken = _jwtService.CreateToken(claims);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            await _userRepository.UpdateUserToken(user, newRefreshToken);

            result.Object = new AuthenticateResponseVM() { Token = newAccessToken, RefreshToken = newRefreshToken };
            return result.Set(true);
        }

        public Task<bool> UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserById(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
