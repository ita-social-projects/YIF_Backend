﻿using AutoMapper;
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
using YIF.Core.Data.Others;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;

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

        public async Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAllUsers()
        {
            var result = new ResponseApiModel<IEnumerable<UserApiModel>>();
            var users = (List<UserDTO>)await _userRepository.GetAll();
            if (users.Count < 1)
            {
                return result.Set(404, $"There are not users in database");
            }
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<UserApiModel>> GetUserById(string id)
        {
            var result = new ResponseApiModel<UserApiModel>();
            try
            {
                var user = await _userRepository.Get(id);
                result.Object = _mapper.Map<UserApiModel>(user);
            }
            catch (KeyNotFoundException ex)
            {
                return result.Set(404, ex.Message);
            }
            return result.Set(true);
        }

        public async Task<ResponseApiModel<IEnumerable<UserApiModel>>> FindUser(Expression<Func<DbUser, bool>> predicate)
        {
            var result = new ResponseApiModel<IEnumerable<UserApiModel>>();
            var foundUsers = await _userRepository.Find(predicate);
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(foundUsers);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> RegisterUser(RegisterApiModel registerModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            var searchUser = _userManager.FindByEmailAsync(registerModel.Email);
            if (searchUser.Result != null)
            {
                return result.Set(409, "User already exist");
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

            var graduate = new Graduate() { User = dbUser };
            var registerResult = await _userRepository.Create(dbUser, graduate, registerModel.Password);

            if (registerResult != string.Empty)
            {
                return result.Set(409, registerResult);
            }

            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _userRepository.UpdateUserToken(dbUser, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            result.Object = new AuthenticateResponseApiModel { Token = token, RefreshToken = refreshToken };

            return result.Set(201);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> LoginUser(LoginApiModel loginModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

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

            result.Object = new AuthenticateResponseApiModel() { Token = token, RefreshToken = refreshToken };

            return result.Set(true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> RefreshToken(TokenRequestApiModel tokenApiModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

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

            result.Object = new AuthenticateResponseApiModel() { Token = newAccessToken, RefreshToken = newRefreshToken };
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





        // =========================   For test authorize endpoint:   =========================
        public async Task<ResponseApiModel<IdByTokenResponseApiModel>> GetCurrentUserIdUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<IdByTokenResponseApiModel>();
            result.Object = new IdByTokenResponseApiModel("Not Valid");

            var token = (await _userManager.Users.Include(u => u.Token).SingleAsync(x => x.Id == id)).Token;
            if (token == null || token.RefreshToken == null || token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return result.Set(false, "Invalid client request");
            }

            result.Object.TokenStatus = "Valid";
            result.Object.Id = id;

            return result.Set(true);
        }

        public async Task<ResponseApiModel<RolesByTokenResponseApiModel>> GetCurrentUserRolesUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<RolesByTokenResponseApiModel>();
            result.Object = new RolesByTokenResponseApiModel("Not Valid");

            var user = await _userManager.Users.Include(u => u.Token).SingleAsync(x => x.Id == id);
            if (user == null || user.Token == null || user.Token.RefreshToken == null)
            {
                return result.Set(false, "Invalid client request");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (user.Token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                result.Object.TokenStatus = "Valid, but expired";
            }

            result.Object.TokenStatus = "Valid";
            result.Object.Roles = await _userManager.GetRolesAsync(user);

            return result.Set(true);
        }

        public async Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAdminsSimilarInstitutionAsCurrentUserUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<IEnumerable<UserApiModel>>();

            var token = (await _userManager.Users.Include(u => u.Token).SingleAsync(x => x.Id == id)).Token;
            if (token == null || token.RefreshToken == null || token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return result.Set(false, "Invalid client request");
            }


            result = await GetAllUsers();
            if (!result.Success)
            {
                return result;
            }
            var allUsers = result.Object;


            var userResult = await GetUserById(id);
            if (!userResult.Success)
            {
                return result.Set(userResult.StatusCode, userResult.Message);
            }
            var currentUser = userResult.Object;


            IEnumerable<UserApiModel> foundAdmins = new List<UserApiModel>();

            if (currentUser.Roles.Contains(ProjectRoles.SchoolModerator))
            {
                foundAdmins = allUsers.Where(u => u.Roles.Contains(ProjectRoles.SchoolAdmin));
            }
            if (currentUser.Roles.Contains(ProjectRoles.UniversityModerator))
            {
                foundAdmins = allUsers.Where(u => u.Roles.Contains(ProjectRoles.UniversityAdmin));
            }
            if (currentUser.Roles.Contains(ProjectRoles.SuperAdmin))
            {
                foundAdmins = allUsers.Where(
                    u => u.Roles.Contains(ProjectRoles.SchoolAdmin) ||
                    u.Roles.Contains(ProjectRoles.UniversityAdmin) ||
                    u.Roles.Contains(ProjectRoles.SchoolModerator) ||
                    u.Roles.Contains(ProjectRoles.UniversityModerator)
                    );
            }


            result.Object = foundAdmins;
            return result.Set(true);
        }
    }
}
