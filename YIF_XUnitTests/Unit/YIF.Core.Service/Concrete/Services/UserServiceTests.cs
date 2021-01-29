using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class UserServiceTests
    {
        private readonly UserService _testService;
        private readonly Mock<IUserRepository<DbUser, UserDTO>> _userRepository;
        private readonly Mock<IUserProfileRepository<UserProfile, UserProfileDTO>> _userProfileRepository;
        private readonly Mock<ITokenRepository> _tokenRepository;
        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly Mock<FakeUserManager<DbUser>> _userManager;
        private readonly FakeSignInManager<DbUser> _signInManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRecaptchaService> _recaptcha;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ISchoolGraduateRepository<SchoolDTO>> _schoolGraduate;
        private readonly Mock<HttpRequest> _request;

        private readonly List<UserApiModel> _listViewModel;
        private readonly List<UserDTO> _listDTO;
        private readonly UserDTO _userDTOStub;
        private readonly UserApiModel _userVMStub;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
            _userProfileRepository = new Mock<IUserProfileRepository<UserProfile, UserProfileDTO>>();
            _tokenRepository = new Mock<ITokenRepository>();
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _serviceProvider = new Mock<IServiceProvider>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _recaptcha = new Mock<IRecaptchaService>();
            _emailService = new Mock<IEmailService>();
            _env = new Mock<IWebHostEnvironment>();
            _configuration = new Mock<IConfiguration>();
            _schoolGraduate = new Mock<ISchoolGraduateRepository<SchoolDTO>>();
            _request = new Mock<HttpRequest>();
            _testService = new UserService(
                _userRepository.Object,
                _userProfileRepository.Object,
                _serviceProvider.Object,
                _userManager.Object,
                _signInManager,
                _jwtService.Object,
                _mapperMock.Object,
                _recaptcha.Object,
                _emailService.Object,
                _env.Object, _configuration.Object,
                _tokenRepository.Object,
                _schoolGraduate.Object);

            _userDTOStub = new UserDTO();
            _userVMStub = new UserApiModel();

            var _userDTOStub2 = new UserDTO
            {
                Id = Guid.NewGuid().ToString("D"),
                UserName = "Jeremiah Gibson",
                Email = "shadj_hadjf@maliberty.com"
            };
            _listDTO = new List<UserDTO> { _userDTOStub, _userDTOStub2 };

            _userVMStub.Id = _userDTOStub.Id = Guid.NewGuid().ToString("D");
            _userVMStub.UserName = _userDTOStub.UserName = "Safwan Wickens";
            _userVMStub.Email = _userDTOStub.Email = "cfarid.nadji2r@devist.com";

            _listViewModel = new List<UserApiModel> {
                new UserApiModel { Id = _listDTO[0].Id, UserName = _listDTO[0].UserName, Email = _listDTO[0].Email },
                new UserApiModel { Id = _listDTO[1].Id, UserName = _listDTO[1].UserName, Email = _listDTO[1].Email }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers_IfThereAreMoreThanOneUser()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAll()).Returns(Task.FromResult(_listDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<UserApiModel>>(_listDTO)).Returns(_listViewModel);
            // Act
            var result = await _testService.GetAllUsers();
            var users = result.Object.ToList();
            //Assert
            Assert.True(result.Success);
            Assert.Equal(users[0].Id, _listViewModel[0].Id);
            Assert.Equal(users[1].Id, _listViewModel[1].Id);
            Assert.Equal(users[1].UserName, _listViewModel[1].UserName);
        }

        [Fact]
        public async Task GetAll_ShouldReturnFalse_IfThereAreNoUsers()
        {
            _userRepository.Setup(s => s.GetAll()).Returns(Task.FromResult(new List<UserDTO>().AsEnumerable()));
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _testService.GetAllUsers());
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserViewModel_WhenUserIsFound()
        {
            // Arrange
            _userRepository.Setup(s => s.Get(_userDTOStub.Id)).Returns(Task.FromResult(_userDTOStub));
            _mapperMock.Setup(s => s.Map<UserApiModel>(_userDTOStub)).Returns(_userVMStub);
            // Act
            var result = await _testService.GetUserById(_userDTOStub.Id);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(_userVMStub.Id, result.Object.Id);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnException_IfUserNotFoundIn()
        {
            // Arrange
            _userRepository.Setup(s => s.Get(It.IsAny<string>()))
                .Throws(new NotFoundException("User not found:  {someId}"));
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _testService.GetUserById(_userDTOStub.Id));
        }

        [Fact]
        public async Task FindUser_ShouldReturnFoundUsers()
        {
            // Arrange
            _userRepository.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Callback<Expression<Func<DbUser, bool>>>(expression => { var func = expression.Compile(); })
                .Returns(() => Task.FromResult(_listDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<UserApiModel>>(_listDTO)).Returns(_listViewModel);
            // Act
            var result = await _testService.FindUser(u => u.Id == _userDTOStub.Id);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Object.Count());
        }

        [Theory]
        [InlineData("test@gmail.com", "test", "PAssword123_", "PAssword123_", "_recaptcha")]
        public async Task RegisterUser_ShouldReturnResponseModelWithToken_WhenDataCorrect(string email, string username, string password, string confirmPassword, string recaptcha)
        {
            // Arrange
            var token = "some correct token";
            var userData = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                RecaptchaToken = recaptcha
            };

            _recaptcha.Setup(x => x.IsValid(userData.RecaptchaToken)).Returns(true);
            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password, ProjectRoles.Graduate)).Returns(Task.FromResult(string.Empty));
            _jwtService.Setup(s => s.SetClaims(It.IsAny<DbUser>())).Verifiable();
            _jwtService.Setup(x => x.CreateToken(It.IsAny<IEnumerable<Claim>>())).Returns(token);
            _signInManager.SignIsSucces = SignInResult.Success;

            // Act
            var result = await _testService.RegisterUser(userData);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Object.Token);
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword123_", "PAssword123_", "recaptcha_", false)]
        [InlineData("test123@gmail.com", "test123", "PAssword123_", "PAssword123_", "recaptcha_", true)]
        public async Task RegisterUser_ShouldReturnExceptionOrApiModelWithUnsuccess_WhenUserWithSameEmailExists(string email, string username, string password, string confirmPassword, string recaptcha, bool isDeleted)
        {
            // Arrange
            var userData = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                RecaptchaToken = recaptcha
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username,
                IsDeleted = isDeleted
            };

            _recaptcha.Setup(x => x.IsValid(userData.RecaptchaToken)).Returns(true);
            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(dbUser));

            if (isDeleted)
            {
                // Act
                var result = await _testService.RegisterUser(userData);
                // Assert
                Assert.False(result.Success);
            }
            else
            {
                // Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => _testService.RegisterUser(userData));
            }
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword123_", "PAssword123_", "recaptcha_")]
        public async Task RegisterUser_ShouldReturnBadRequestWithMessage_WhenUserWithSameNameExists(string email, string username, string password, string confirmPassword, string recaptcha)
        {
            // Arrange
            var message = "message";

            var userData = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                RecaptchaToken = recaptcha
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username
            };

            _recaptcha.Setup(x => x.IsValid(userData.RecaptchaToken)).Returns(true);
            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userProfileRepository.Setup(x => x.SetDefaultUserProfileIfEmpty(It.IsAny<string>())).Verifiable();
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password, ProjectRoles.Graduate)).Returns(Task.FromResult(message));

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _testService.RegisterUser(userData));
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword122_", "PAssword123_", "recaptcha_", "Паролі не співпадають!")]
        [InlineData("test123@gmail.com", "test123", "PAssword12", "PAssword12", "recaptcha_", "Пароль має містити щонайменше один спеціальний символ!")]
        [InlineData("test123@gmail.com", "test123", "pass", "pass", "recaptcha_", "Пароль має містити мінімум 8 символів і максимум 20 (включно)!")]
        public async Task RegisterUser_ShouldReturnBadRequestException_WhenPasswordIsBad(string email, string username, string password, string confirmPassword, string recaptcha, string message)
        {
            // Arrange
            var userData = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                RecaptchaToken = recaptcha
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username
            };

            _recaptcha.Setup(x => x.IsValid(userData.RecaptchaToken)).Returns(true);
            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password, ProjectRoles.Graduate)).Returns(Task.FromResult(message));

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _testService.RegisterUser(userData));
        }

        [Fact]
        public async Task LoginUser_ShouldReturnResponseModelWithToken_WhenEmailAndPasswordAreCorrect()
        {
            // Arrange
            var token = "some correct token";
            var loginAM = new LoginApiModel
            {
                Email = "necum.how@silentsuite.com",
                Password = "QWerty-1",
                RecaptchaToken = "recaptcha"
            };
            var user = new DbUser
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = loginAM.Email,
                PasswordHash = loginAM.Password
            };

            _recaptcha.Setup(x => x.IsValid(loginAM.RecaptchaToken)).Returns(true);
            _userManager.Setup(s => s.FindByEmailAsync(loginAM.Email)).ReturnsAsync(user);
            _userManager.Setup(s => s.CheckPasswordAsync(user, loginAM.Password)).ReturnsAsync(true);
            _signInManager.SignIsSucces = Microsoft.AspNetCore.Identity.SignInResult.Success;
            _jwtService.Setup(s => s.SetClaims(It.IsAny<DbUser>())).Verifiable();
            _jwtService.Setup(s => s.CreateToken(It.IsAny<IEnumerable<Claim>>())).Returns(token);
            _jwtService.Setup(s => s.CreateRefreshToken()).Returns(token);
            _tokenRepository.Setup(x => x.UpdateUserToken(user, token)).Verifiable();
            // Act
            var result = await _testService.LoginUser(loginAM);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Object.Token);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1", "recaptcha")]
        //[InlineData("qtoni6@gmail.com", "d", "recaptcha")]
        //[InlineData("", "", "")]
        public async Task LoginUser_ShouldReturnFalse_WhenEmailOrPasswordAreIncorrect(string email, string password, string recaptcha)
        {
            // Arrange
            var loginVM = new LoginApiModel { Email = email, Password = password, RecaptchaToken = recaptcha };
            var user = new DbUser { Id = Guid.NewGuid().ToString("D"), Email = email, PasswordHash = password };

            _recaptcha.Setup(x => x.IsValid(loginVM.RecaptchaToken)).Returns(true);
            _userManager.Setup(s => s.FindByEmailAsync(email)).ReturnsAsync(email == "" ? null : user);
            _signInManager.SignIsSucces = Microsoft.AspNetCore.Identity.SignInResult.Failed;

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _testService.LoginUser(loginVM));
        }

        [Fact]
        public void DeleteUser_ShouldReturnException_Yet()
        {
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(() => _testService.DeleteUserById(_userDTOStub.Id));
        }

        [Fact]
        public void UpdateUser_ShouldReturnException_Yet()
        {
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(() => _testService.UpdateUser(_userDTOStub));
        }

        [Theory]
        [InlineData("email@gmail.com")]
        public async void Send_ResetPasswordEmail(string email)
        {
            // Arrange
            var userModel = new DbUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email
            };

            _request.SetupAllProperties();
            _userManager.Setup(s => s.FindByEmailAsync(email)).ReturnsAsync(userModel);
            _emailService.Setup(s => s.SendAsync(email, "", ""))
                .ReturnsAsync(null);
            _serviceProvider.Setup(s => s.GetService(typeof(UserManager<DbUser>))).Returns(_userManager.Object);

            // Act
            var result = await _testService.ResetPasswordByEmail(email, _request.Object);

            // Assert
            Assert.True(result.Success);
        }

        [Theory]
        [InlineData("anataly@gmail.com")]
        public async void Send_ConfirmEmail_Mail(string email)
        {
            // Arrange
            var apiModel = new EmailApiModel
            {
                UserEmail = email
            };

            var userModel = new DbUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email
            };

            _request.SetupAllProperties();
            _userManager.Setup(s => s.FindByEmailAsync(userModel.Email)).ReturnsAsync(userModel);
            _userManager.Setup(s => s.GenerateEmailConfirmationTokenAsync(userModel)).Returns(Task.FromResult("Token"));
            _emailService.Setup(s => s.SendAsync(apiModel.UserEmail, "", ""))
                .ReturnsAsync(null);
            // Act
            var result = await _testService.SendEmailConfirmMail(apiModel, _request.Object);

            // Assert
            Assert.True(result.Success);
        }

        [Theory]
        [InlineData("34cc87d9-6d76-44ac-9dda-15852feb9e72", "token")]
        public async void ConfirmUserEmail(string id, string token)
        {
            // Arrange
            var apiModel = new ConfirmEmailApiModel
            {
                Id = id,
                Token = token
            };
            var userModel = new DbUser
            {
                Id = id
            };

            _userManager.Setup(s => s.FindByIdAsync(userModel.Id)).ReturnsAsync(userModel);
            _userManager.Setup(s => s.ConfirmEmailAsync(userModel, token)).Returns(Task.FromResult(new IdentityResult()));
            // Act
            var result = await _testService.ConfirmUserEmail(apiModel);

            // Assert
            Assert.True(result.Success);
        }

        [Theory]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-1", "QWerty-12", "QWerty-12",
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async void ChangePassword_WithCorrectData(string id,
            string oldPassword,
            string newPassword,
            string confirmPassword,
            string recaptcha)
        {
            // Arrange
            var model = new ChangePasswordApiModel
            {
                UserId = id,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword,
                RecaptchaToken = recaptcha
            };
            var user = new DbUser
            {
                Id = Guid.NewGuid().ToString()
            };

            _recaptcha.Setup(x => x.IsValid(model.RecaptchaToken)).Returns(true);
            _userManager.Setup(s => s.FindByIdAsync(model.UserId)).ReturnsAsync(user);
            _userManager.Setup(s => s.ChangePasswordAsync(user, model.OldPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(s => s.CheckPasswordAsync(user, model.OldPassword)).ReturnsAsync(true);

            // Act
            var myResult = await _testService.ChangeUserPassword(model);

            // Assert
            Assert.True(myResult.Success);
        }

        [Fact]
        public void Dispose_ShouldDisposeRepositories()
        {
            // Arrange
            var userRepo = new Mock<IUserRepository<DbUser, UserDTO>>();
            var profileRepo = new Mock<IUserProfileRepository<UserProfile, UserProfileDTO>>();
            var userResult = false;
            var profileResult = false;
            userRepo.Setup(x => x.Dispose()).Callback(() => userResult = true);
            profileRepo.Setup(x => x.Dispose()).Callback(() => profileResult = true);
            // Act
            var service = new UserService(userRepo.Object, profileRepo.Object, null, null, null, null, null, null, null, null, null, null, null);
            service.Dispose();
            // Assert
            Assert.True(userResult);
            Assert.True(profileResult);
        }
    }
}
