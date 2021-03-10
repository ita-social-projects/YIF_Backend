using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services.ValidatorServices;
using YIF.Shared;

namespace YIF.Core.Service.Concrete.Services
{
    public class UserService : IUserService<DbUser>
    {
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly IUserProfileRepository<UserProfile, UserProfileDTO> _userProfileRepository;
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly ITokenRepository<TokenDTO> _tokenRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRecaptchaService _recaptcha;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ISchoolGraduateRepository<SchoolDTO> _schoolGraduateRepository;
        private readonly ResourceManager _resourceManager;

        public UserService(
            IUserRepository<DbUser, UserDTO> userRepository,
            IUserProfileRepository<UserProfile, UserProfileDTO> userProfileRepository,
            ISchoolRepository<SchoolDTO> schoolRepository,
            IServiceProvider serviceProvider,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IRecaptchaService recaptcha,
            IEmailService emailService,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ITokenRepository<TokenDTO> tokenRepository,
            ISchoolGraduateRepository<SchoolDTO> schoolGraduate,
            ResourceManager resourceManager)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _schoolRepository = schoolRepository;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _recaptcha = recaptcha;
            _emailService = emailService;
            _env = env;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
            _schoolGraduateRepository = schoolGraduate;
            _resourceManager = resourceManager;
        }

        public async Task<ResponseApiModel<IEnumerable<UserApiModel>>> GetAllUsers()
        {
            var result = new ResponseApiModel<IEnumerable<UserApiModel>>();
            var users = (List<UserDTO>)await _userRepository.GetAll();
            if (users.Count < 1)
            {
                throw new NotFoundException(_resourceManager.GetString("UsersNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(users);
            
            return result.Set(true);
        }

        public async Task<ResponseApiModel<UserApiModel>> GetUserById(string id)
        {
            var result = new ResponseApiModel<UserApiModel>();
            var user = await _userRepository.Get(id);
            if (user == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserNotFound")}: {id}");
            }
            result.Object = _mapper.Map<UserApiModel>(user);
            
            return result.Set(true);
        }

        public async Task<ResponseApiModel<UserApiModel>> GetUserByEmail(string email)
        {
            var result = new ResponseApiModel<UserApiModel>();
            var user = await _userRepository.GetByEmail(email);
            if (user == null)
            {                
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchEmailNotFound")}: {email}");
            }
            result.Object = _mapper.Map<UserApiModel>(user);
            
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
                throw new BadRequestException(validResults.ToString());
            }

            var searchUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (searchUser != null)
            {
                if (searchUser.IsDeleted == false)
                {
                    throw new InvalidOperationException("Електронна пошта вже використовувалась раніше. Якщо це ваша, авторизуйтесь або скористайтесь відновленням доступу");
                }
                return result.Set(false, "Електронна пошта вже використовувалась раніше. Якщо це ваша, авторизуйтесь або скористайтесь відновленням доступу");
            }

            searchUser = await _userManager.FindByNameAsync(registerModel.Username);
            if (searchUser != null) 
                throw new BadRequestException(_resourceManager.GetString("UsernameAlreadyExists"));

            var dbUser = new DbUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Username
            };

            var graduate = new Graduate { UserId = dbUser.Id };
            var registerResult = await _userRepository.Create(dbUser, graduate, registerModel.Password, ProjectRoles.Graduate);
            if (registerResult != string.Empty)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            await _userProfileRepository.SetDefaultUserProfileIfEmpty(dbUser.Id);

            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            var tokenDb = await _tokenRepository.FindUserToken(dbUser.Id);
            if (tokenDb == null)
            {
                tokenDb = new TokenDTO
                {
                    Id = dbUser.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                };
                await _tokenRepository.AddUserToken(tokenDb);
            }
            else await _tokenRepository.UpdateUserToken(dbUser.Id, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> LoginUser(LoginApiModel loginModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            var validator = new LoginValidator(_userManager, _recaptcha);
            var validResults = validator.Validate(loginModel);

            if (!validResults.IsValid)
            {
                throw new BadRequestException(validResults.ToString());
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {                
                throw new BadRequestException(_resourceManager.GetString("LoginOrPasswordIncorrect"));
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (!loginResult.Succeeded)
            {
                throw new BadRequestException(_resourceManager.GetString("LoginOrPasswordIncorrect"));
            }

            var token = _jwtService.CreateToken(_jwtService.SetClaims(user));
            var refreshToken = _jwtService.CreateRefreshToken();

            var tokenDb = await _tokenRepository.FindUserToken(user.Id);
            if (tokenDb == null)
            {
                tokenDb = new TokenDTO
                {
                    Id = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                };
                await _tokenRepository.AddUserToken(tokenDb);
            }
            else await _tokenRepository.UpdateUserToken(user.Id, refreshToken);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> RefreshToken(TokenRequestApiModel tokenApiModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            string accessToken = tokenApiModel.Token;
            string refreshToken = tokenApiModel.RefreshToken;

            var claims = _jwtService.GetClaimsFromExpiredToken(accessToken);
            if (claims == null)
            {                
                throw new BadRequestException(_resourceManager.GetString("InvalidToken"));
            }

            var userId = claims.First(claim => claim.Type == "id").Value;
            var user = await _userRepository.GetUserWithToken(userId);

            if (user == null)
            {
                throw new BadRequestException(_resourceManager.GetString("UserDoesNotExist"));
            }

            if (user.Token == null)
            {                
                throw new BadRequestException(_resourceManager.GetString("YouMustLogInFirst"));
            }

            if (user.Token.RefreshTokenExpiryTime <= DateTime.Now)
            {                
                throw new BadRequestException(_resourceManager.GetString("RefreshTokenExpired"));
            }

            var newAccessToken = _jwtService.CreateToken(claims);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            var tokenDb = await _tokenRepository.FindUserToken(user.Id);
            if (tokenDb == null)
            {
                tokenDb = new TokenDTO
                {
                    Id = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                };
                await _tokenRepository.AddUserToken(tokenDb);
            }
            else await _tokenRepository.UpdateUserToken(user.Id, refreshToken);

            return result.Set(new AuthenticateResponseApiModel(newAccessToken, newRefreshToken), true);
        }

        public async Task<ResponseApiModel<UserProfileApiModel>> GetUserProfileInfoById(string userId, HttpRequest request)
        {
            var userDto = await _userRepository.GetUserWithUserProfile(userId);
            if (userDto == null)
            {
                throw new NotFoundException(_resourceManager.GetString("UserNotFound"));
            }

            var userProfile = _mapper.Map<UserProfileApiModel>(userDto.UserProfile);

            string pathPhoto = $"{request.Scheme}://{request.Host}/{_configuration.GetValue<string>("UrlImages")}/";
            userProfile.Photo = userProfile.Photo != null ? pathPhoto + userProfile.Photo : null;

            var schoolDto = await _schoolGraduateRepository.GetSchoolByUserId(userId);
            userProfile.SchoolName = schoolDto?.Name;

            return new ResponseApiModel<UserProfileApiModel>(userProfile, true);
        }

        public async Task<ResponseApiModel<UserProfileWithoutPhotoApiModel>> SetUserProfileInfoById(UserProfileWithoutPhotoApiModel model, string userId)
        {
            var result = new ResponseApiModel<UserProfileWithoutPhotoApiModel>();

            var profile = _mapper.Map<UserProfileDTO>(model);
            profile.Id = userId;

            if (!await _userRepository.Exist(profile.Id))
            {
                throw new KeyNotFoundException("Користувача не знайдено із таким id:  " + profile.Id);
            }

            // Check email (Is newEmail exist in another user or not)
            if (await _userProfileRepository.EmailExistInAnotherUser(profile.Email, profile.Id))
            {
                throw new ArgumentException("Електронна пошта вже використовується:  " + profile.Email);
            }

            // Set school
            if (!await _userProfileRepository.IsCurrentUserTheGraduate(profile.Id))
            {
                if (!string.IsNullOrWhiteSpace(model.SchoolName)) throw new ArgumentException("Користувач не належить до ролі: " + ProjectRoles.Graduate +
                    ". Поле 'schoolname' не потрібно заповняти для цього коритувача.");
            }
            else
            {
                if (!await _schoolRepository.Exist(model.SchoolName))
                {
                    throw new ArgumentException("Не знайдено зазначеної школи:  " + model.SchoolName);
                }
                await _userProfileRepository.SetSchoolForGraduate(model.SchoolName, profile.Id);
            }

            var profileDTO = await _userProfileRepository.SetUserProfile(profile);
            result.Object = _mapper.Map<UserProfileWithoutPhotoApiModel>(profileDTO);
            
            return result.Set(true);
        }

        public async Task<ResponseApiModel<ImageApiModel>> GetUserPhoto(string userId, HttpRequest request)
        {
            var user = await _userRepository.GetUserWithUserProfile(userId);
            string pathPhoto = $"{request.Scheme}://{request.Host}/{_configuration.GetValue<string>("UrlImages")}/";
            string imagePath = user?.UserProfile?.Photo != null ? pathPhoto + user.UserProfile.Photo : null;
            
            return new ResponseApiModel<ImageApiModel>(new ImageApiModel { Photo = imagePath }, true);
        }

        public async Task<ResponseApiModel<ImageApiModel>> ChangeUserPhoto(ImageApiModel model, string userId, HttpRequest request)
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
                throw new BadRequestException(_resourceManager.GetString("PhotoNotChanged"));
            }

            if (File.Exists(filePathDelete))
            {
                File.Delete(filePathDelete);
            }

            string pathPhoto = $"{request.Scheme}://{request.Host}/{_configuration.GetValue<string>("UrlImages")}/";
            fileName = fileName != null ? pathPhoto + fileName : null;

            return new ResponseApiModel<ImageApiModel>(new ImageApiModel { Photo = fileName }, true);
        }

        public Task<bool> UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserById(string id)
        {
            return await _userRepository.Delete(id);
        }

        public void Dispose()
        {
            _userRepository.Dispose();
            _userProfileRepository.Dispose();
        }


        public async Task<ResponseApiModel<bool>> ResetPasswordByEmail(string userEmail, HttpRequest request)
        {
            var result = new ResponseApiModel<bool>();

            var validResults = new EmailModelValidator().Validate(userEmail);
            if (!validResults.IsValid) 
                return result.Set(false, validResults.ToString());

            var manager = (UserManager<DbUser>)_serviceProvider.GetService(typeof(UserManager<DbUser>));
            var user = await manager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                var token = await manager.GeneratePasswordResetTokenAsync(user);
                token = System.Web.HttpUtility.UrlEncode(token);

                var serverUrl = $"{request.Scheme}://{request.Host}/";
                var url = serverUrl + $"restore?id={user.Id}&token={token}";

                var topic = user.IsDeleted ?
                    _resourceManager.GetString("AccountRecovery") :
                    _resourceManager.GetString("PasswordRecovery");

                var type = user.IsDeleted ?
                    _resourceManager.GetString("RestoreAccount") :
                    _resourceManager.GetString("RecoverPassword");

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
<td style=""border-radius: 3px;"" align=""center"" bgcolor=""#FFA73B"">
<a style=""font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;"" href=""{url}"" target=""_blank"" rel=""noopener"">{type}</a>
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

                await _emailService.SendAsync(userEmail, topic, html);
            }

            return result.Set(true, _resourceManager.GetString("AccountRecoveryInstructions"));
        }

        public async Task<ResponseApiModel<bool>> RestorePasswordById(RestoreApiModel model)
        {
            var result = new ResponseApiModel<bool>();

            var validator = new ResetValidator(_userManager, _recaptcha);
            var validResults = validator.Validate(model);

            if (!validResults.IsValid)
            {
                throw new ArgumentException(validResults.ToString());
            }

            var manager = (UserManager<DbUser>)_serviceProvider.GetService(typeof(UserManager<DbUser>));
            var user = await manager.FindByIdAsync(model.UserId);
            if (user == null)
            {                
                return result.Set(false, _resourceManager.GetString("UserNotFound"));
            }

            var token = System.Web.HttpUtility.UrlDecode(model.Token);
            var restoreResult = await manager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!restoreResult.Succeeded)
            {
                throw new ArgumentException(restoreResult.Errors.First().Description);
            }
            
            result.Message = user.IsDeleted ?
                _resourceManager.GetString("AccountSuccessfullyRestored") :
                _resourceManager.GetString("PasswordSuccessfullyRecovered");

            if (user.IsDeleted) user.IsDeleted = false;
            await manager.UpdateAsync(user);

            return result.Set(true);
        }
        public async Task<ResponseApiModel<ChangePasswordApiModel>> ChangeUserPassword(ChangePasswordApiModel model)
        {
            var result = new ResponseApiModel<ChangePasswordApiModel>();

            var validator = new ChangePasswordValidator(_userManager, _recaptcha);
            var validResults = validator.Validate(model);

            if (!validResults.IsValid)
            {
                throw new ArgumentException(validResults.ToString());
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            var changeResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changeResult.Succeeded)
            {                
                throw new InvalidOperationException(_resourceManager.GetString("ErrorOccurred"));
            }

            result.Object = model;

            return result.Set(true);
        }

        public async Task<ResponseApiModel<bool>> SendEmailConfirmMail(EmailApiModel model, HttpRequest request)
        {
            var result = new ResponseApiModel<bool>();

            var validResults = new EmailModelValidator().Validate(model.UserEmail);
            if (!validResults.IsValid) 
                return result.Set(false, validResults.ToString());

            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            if (user == null || user.IsDeleted)
            {                
                throw new NotFoundException(_resourceManager.GetString("EmailIsNotActive"));
            }

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var serverUrl = $"{request.Scheme}://{request.Host}/";
            var url = serverUrl + $"email/approve?id={user.Id}&token={confirmToken}";
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
<h1 style=""font-size: 48px; font-weight: 400; margin: 2;"">Підтвердження пошти</h1>
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
<td style=""border-radius: 3px;"" align=""center"" bgcolor=""#FFA73B"">
<a style=""font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;"" href=""{url}"" target=""_blank"" rel=""noopener"">Підтвердити</a>
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

            await _emailService.SendAsync(model.UserEmail, "Підтвердження пошти", html);

            return result.Set(true, _resourceManager.GetString("EmailConfirmationInstructions"));
        }
        public async Task<ResponseApiModel<ConfirmEmailApiModel>> ConfirmUserEmail(ConfirmEmailApiModel model)
        {
            var result = new ResponseApiModel<ConfirmEmailApiModel>();
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null || user.IsDeleted)
            {
                throw new NotFoundException(_resourceManager.GetString("EmailIsNotActive"));
            }

            if (user.EmailConfirmed)
            {                
                throw new ArgumentException(_resourceManager.GetString("EmailAlreadyConfirmed"));
            }

            await _userManager.ConfirmEmailAsync(user, model.Token);

            result.Object = model;

            return result.Set(true);
        }

        // =========================   For test authorize endpoint:   =========================

        public async Task<ResponseApiModel<RolesByTokenResponseApiModel>> GetCurrentUserRolesUsingAuthorize(string id)
        {
            var result = new ResponseApiModel<RolesByTokenResponseApiModel>(new RolesByTokenResponseApiModel("Not Valid"), false);

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

            var users = (await GetAllUsers()).Object;
            result.Object = users.Where(
                    u => u.Roles.Contains(ProjectRoles.SchoolAdmin) ||
                    u.Roles.Contains(ProjectRoles.UniversityAdmin)
                    );
            if (result.Object.Count() < 0)
            {
                throw new NotFoundException(_resourceManager.GetString("AdminsNotFound"));
            }
            return result.Object.Count() > 0 ? result.Set(true) : result.Set(false, _resourceManager.GetString("AdminsNotFound"));
        }
    }
}
