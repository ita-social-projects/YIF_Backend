using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class UserService : IUserService<DbUser>
    {
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRecaptchaService _recaptcha;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IRecaptchaService recaptcha,
            IEmailService emailService,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _recaptcha = recaptcha;
            _emailService = emailService;
            _env = env;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
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

        public async Task<ResponseApiModel<UserApiModel>> GetUserByEmail(string email)
        {
            var result = new ResponseApiModel<UserApiModel>();
            try
            {
                var user = await _userRepository.GetByEmail(email);
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
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
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

            var graduate = new Graduate { UserId = dbUser.Id };
            var registerResult = await _userRepository.Create(dbUser, graduate, registerModel.Password, ProjectRoles.Graduate);

            await _userRepository.SetDefaultUserProfileIfEmpty(dbUser.Id);

            if (registerResult != string.Empty)
            {
                return result.Set(409, registerResult);
            }

            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _tokenRepository.UpdateUserToken(dbUser, refreshToken);

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

            await _tokenRepository.UpdateUserToken(user, refreshToken);

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
                return result.Set(false, "Invalid client request. Invalid token.");
            }

            var userId = claims.First(claim => claim.Type == "id").Value;

            var user = await _userRepository.GetUserWithToken(userId);

            if (user == null)
            {
                return result.Set(false, "Invalid client request. User doesn't exist.");
            }

            if (user.Token == null || user.Token.RefreshToken != refreshToken)
            {
                return result.Set(false, "Invalid client request. You must log in first.");
            }

            if (user.Token.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return result.Set(false, "Invalid client request. Refresh token expired.");
            }

            var newAccessToken = _jwtService.CreateToken(claims);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            await _tokenRepository.UpdateUserToken(user, newRefreshToken);

            result.Object = new AuthenticateResponseApiModel() { Token = newAccessToken, RefreshToken = newRefreshToken };
            return result.Set(true);
        }

        public async Task<UserProfileDTO> GetUserProfileInfoById(string userId)
        {
            var user = await _userRepository.GetUserWithUserProfile(userId);
            return _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<ResponseApiModel<UserProfileApiModel>> SetUserProfileInfoById(UserProfileApiModel model, string userId)
        {
            var result = new ResponseApiModel<UserProfileApiModel>(false, "Профіль користувача не встановлено.");

            var newEmail = model.Email;
            model.Email = (await _userManager.FindByIdAsync(userId)).Email;
            var profile = _mapper.Map<UserProfile>(model);
            profile.User.Email = newEmail;

            try
            {
                var user = await _userRepository.SetUserProfile(profile, userId, model.SchoolName);
                result.Object = _mapper.Map<UserProfileApiModel>(user);
                return result.Set(true);
            }
            catch (KeyNotFoundException ex)
            {
                return result.Set(false, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return result.Set(false, ex.Message);
            }
        }

        public async Task<ImageApiModel> ChangeUserPhoto(ImageApiModel model, string userId)
        {
            var user = await _userRepository.GetUserWithUserProfile(userId);

            string base64 = model.Photo;
            if (base64.Contains(","))
            {
                base64 = base64.Split(',')[1];
            }

            var serverPath = _env.ContentRootPath; //Directory.GetCurrentDirectory(); //_env.WebRootPath;
            var folerName = _configuration.GetValue<string>("ImagesPath");
            var path = Path.Combine(serverPath, folerName);

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            string ext = ".jpg";
            string fileName = Guid.NewGuid().ToString("D") + ext;
            string filePathSave = Path.Combine(path, fileName);

            string filePathDelete = null;
            if (user.UserProfile != null && user.UserProfile.Photo != null)
                filePathDelete = Path.Combine(path, user.UserProfile.Photo);

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(filePathSave, imageBytes);

            var result = await _userRepository.UpdateUserPhoto(user, fileName);

            if (!result)
            {
                return null;
            }

            if (File.Exists(filePathDelete))
            {
                File.Delete(filePathDelete);
            }

            return new ImageApiModel { Photo = fileName };
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

        public async Task<ResponseApiModel<ResetPasswordByEmailApiModel>> ResetPasswordByEmail(ResetPasswordByEmailApiModel model)
        {
            var result = new ResponseApiModel<ResetPasswordByEmailApiModel>();
            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            if (user == null)
            {
                return result.Set(404, "Такий емейл не є зареєстрованим");
            }

            var url = "url";
            var html = $@"<p>&nbsp;</p>
<!-- HIDDEN PREHEADER TEXT -->
<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0""><!-- LOGO -->
<tbody>
<tr>
<td align=""center"" bgcolor=""#FFA73B"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""padding: 40px 10px 40px 10px;"" align=""center"" valign=""top"">&nbsp;</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#FFA73B"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;"" align=""center"" valign=""top"" bgcolor=""#ffffff"">
<h1 style=""font-size: 48px; font-weight: 400; margin: 2;"">Відновлення паролю</h1>
<img style=""display: block; border: 0px;"" src="" https://img.icons8.com/clouds/100/000000/handshake.png"" width=""125"" height=""120"" /></td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">
<table style=""max-width: 600px;"" border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td align=""left"" bgcolor=""#ffffff"">
<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr style=""height: 39px;"">
<td style=""padding: 20px 30px 60px; height: 39px;"" align=""center"" bgcolor=""#ffffff"">
<table border=""0"" cellspacing=""0"" cellpadding=""0"">
<tbody>
<tr>
<td style=""border-radius: 3px;"" align=""center"" bgcolor=""#FFA73B""><a style=""font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;"" href=""{url}"" target=""_blank"" rel=""noopener"">Змінити пароль</a></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style=""padding: 30px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">&nbsp;</td>
</tr>
<tr>
<td style=""padding: 0px 10px 0px 10px;"" align=""center"" bgcolor=""#f4f4f4"">&nbsp;</td>
</tr>
</tbody>
</table>";
            _emailService.SendAsync(model.UserEmail,"Відновлення паролю", html);

            result.Object = model;

            return result.Set(true);
        }
    }
}
