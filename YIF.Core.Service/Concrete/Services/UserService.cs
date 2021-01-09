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
        private readonly IRecaptchaService _recaptcha;
        private readonly IEmailService _emailService;

        public UserService(IRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IRecaptchaService recaptcha,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _recaptcha = recaptcha;
            _emailService = emailService;
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

            var validator = new RegisterValidator(_userManager, _recaptcha);
            var validResults = validator.Validate(registerModel);

            if (!validResults.IsValid)
            {
                return result.Set(false, validResults.ToString());
            }

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

            var graduate = new Graduate();
            var registerResult = await _userRepository.Create(dbUser, graduate, registerModel.Password, ProjectRoles.Graduate);

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

            var validator = new LoginValidator(_userManager, _recaptcha);
            var validResults = validator.Validate(loginModel);

            if (!validResults.IsValid)
            {
                return result.Set(false, validResults.ToString());
            }

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

            await _emailService.SendAsync("ivanna.pugaiko@gmail.com", "Sending email is Fun", "<strong>Blazor Web Apps - Goodbye JavaScript! I'm in love with C#</strong>");

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
        
        public async Task<ResponseApiModel<RolesByTokenResponseApiModel>> GetCurrentUserRolesUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<RolesByTokenResponseApiModel>();
            result.Object = new RolesByTokenResponseApiModel("Not Valid");

            var user = await _userManager.Users.Include(u => u.Token).SingleAsync(x => x.Id == id);

            var roles = await _userManager.GetRolesAsync(user);

            if (user.Token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                result.Object.TokenStatus = "Valid, but expired";
            }

            result.Object.TokenStatus = "Valid";
            result.Object.Roles = await _userManager.GetRolesAsync(user);

            return result.Set(true);
        }

        public async Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAdminsUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<IEnumerable<UserApiModel>>();

            result = await GetAllUsers();
            if (!result.Success)
            {
                return result;
            }

            return result.Set(200, result.Object.Where(
                    u => u.Roles.Contains(ProjectRoles.SchoolAdmin) ||
                    u.Roles.Contains(ProjectRoles.UniversityAdmin)
                    ));
        }
    }
}
