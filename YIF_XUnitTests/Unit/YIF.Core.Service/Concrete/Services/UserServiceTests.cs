using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF.Core.Domain.ViewModels.UserViewModels;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class UserServiceTests
    {
        private readonly UserService _testService;
        private readonly Mock<IRepository<DbUser, UserDTO>> _userRepository;
        private readonly Mock<FakeUserManager<DbUser>> _userManager;
        private readonly FakeSignInManager<DbUser> _signInManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IMapper> _mapperMock;

        private readonly List<UserViewModel> _listViewModel;
        private readonly List<UserDTO> _listDTO;
        private readonly UserDTO _userDTOStub;
        private readonly UserViewModel _userVMStub;

        public UserServiceTests()
        {
            _userRepository = new Mock<IRepository<DbUser, UserDTO>>();
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _testService = new UserService(_userRepository.Object, _userManager.Object, _signInManager, _jwtService.Object, _mapperMock.Object);

            _userDTOStub = new UserDTO();
            _userVMStub = new UserViewModel();

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

            _listViewModel = new List<UserViewModel> {
                new UserViewModel { Id = _listDTO[0].Id, UserName = _listDTO[0].UserName, Email = _listDTO[0].Email },
                new UserViewModel { Id = _listDTO[1].Id, UserName = _listDTO[1].UserName, Email = _listDTO[1].Email }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers_IfThereAreMoreThanOneUser()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAll()).Returns(Task.FromResult(_listDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<UserViewModel>>(_listDTO)).Returns(_listViewModel);
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
            // Act
            var result = await _testService.GetAllUsers();
            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserViewModel_WhenUserIsFound()
        {
            // Arrange
            _userRepository.Setup(s => s.Get(_userDTOStub.Id)).Returns(Task.FromResult(_userDTOStub));
            _mapperMock.Setup(s => s.Map<UserViewModel>(_userDTOStub)).Returns(_userVMStub);
            // Act
            var result = await _testService.GetUserById(_userDTOStub.Id);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(_userVMStub.Id, result.Object.Id);
        }

        [Fact]
        public void GetUserById_ShouldReturnException_IfUserNotFoundIn()
        {
            // Arrange
            _userRepository.Setup(s => s.Get(It.IsAny<string>()))
                .Throws(new KeyNotFoundException("User not found:  {someId}"));
            // Act
            var result = _testService.GetUserById(_userDTOStub.Id).Result;
            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Object);
            Assert.StartsWith("User not found", result.Message);
        }

        [Fact]
        public async Task FindUser_ShouldReturnFoundUsers()
        {
            // Arrange
            _userRepository.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Callback<Expression<Func<DbUser, bool>>>(expression => { var func = expression.Compile(); })
                .Returns(() => Task.FromResult(_listDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<UserViewModel>>(_listDTO)).Returns(_listViewModel);
            // Act
            var result = await _testService.FindUser(u => u.Id == _userDTOStub.Id);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Object.Count());
        }

        [Theory]
        [InlineData("test@gmail.com", "test", "PAssword123_", "PAssword123_")]
        public async Task RegisterUser_ShouldReturnResponseModelWithToken_WhenDataCorrect(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var token = "some correct token";
            var userData = new RegisterViewModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password)).Returns(Task.FromResult(string.Empty));
            _jwtService.Setup(x => x.CreateTokenByUser(It.IsAny<DbUser>())).Returns(token);
            _signInManager.SignIsSucces = Microsoft.AspNetCore.Identity.SignInResult.Success;

            // Act
            var result = await _testService.RegisterUser(userData);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Object.Token);
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword123_", "PAssword123_")]
        public async Task RegisterUser_ShouldReturnBadRequestWithMessage_WhenUserWithSameEmailExists(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var userData = new RegisterViewModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username
            };

            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(dbUser));

            // Act
            var result = await _testService.RegisterUser(userData);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Object);
            Assert.Equal("User already exist", result.Message);
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword123_", "PAssword123_")]
        public async Task RegisterUser_ShouldReturnBadRequestWithMessage_WhenUserWithSameNameExists(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var message = $"User name '{username}' is already taken.";

            var userData = new RegisterViewModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username
            };

            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password)).Returns(Task.FromResult(message));

            // Act
            var result = await _testService.RegisterUser(userData);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Object);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("test123@gmail.com", "test123", "PAssword122_", "PAssword123_", "Password and confirm password does not compare")]
        [InlineData("test123@gmail.com", "test123", "PAssword12", "PAssword12", "Passwords must have at least one non alphanumeric character.")]
        [InlineData("test123@gmail.com", "test123", "pass", "pass", "Passwords must be at least 6 characters.")]
        public async Task RegisterUser_ShouldReturnBadRequestWithMessage_WhenPasswordIsBad(string email, string username, string password, string confirmPassword, string message)
        {
            // Arrange
            var userData = new RegisterViewModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var dbUser = new DbUser
            {
                Email = email,
                UserName = username
            };

            _userManager.Setup(x => x.FindByEmailAsync(userData.Email)).Returns(Task.FromResult<DbUser>(null));
            _userRepository.Setup(x => x.Create(It.IsAny<DbUser>(), It.IsAny<object>(), userData.Password)).Returns(Task.FromResult(message));

            // Act
            var result = await _testService.RegisterUser(userData);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Object);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnResponseModelWithToken_WhenEmailAndPasswordAreCorrect()
        {
            // Arrange
            var token = "some correct token";
            var loginVM = new LoginViewModel { Email = "email@gmail.com", Password = "password" };
            var user = new DbUser { Id = Guid.NewGuid().ToString("D"), Email = loginVM.Email, PasswordHash = loginVM.Password };
            _userManager.Setup(s => s.FindByEmailAsync(loginVM.Email)).ReturnsAsync(user);
            _signInManager.SignIsSucces = Microsoft.AspNetCore.Identity.SignInResult.Success;
            _jwtService.Setup(s => s.CreateTokenByUser(user)).Returns(token);
            // Act
            var result = await _testService.LoginUser(loginVM);
            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Object.Token);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1")]
        [InlineData("qtoni6@gmail.com", "d")]
        [InlineData("", "")]
        public async Task LoginUser_ShouldReturnFalse_WhenEmailOrPasswordAreIncorrect(string email, string password)
        {
            // Arrange
            var loginVM = new LoginViewModel { Email = email, Password = password };
            var user = new DbUser { Id = Guid.NewGuid().ToString("D"), Email = email, PasswordHash = password };
            _userManager.Setup(s => s.FindByEmailAsync(email)).ReturnsAsync(email == "" ? null : user);
            _signInManager.SignIsSucces = Microsoft.AspNetCore.Identity.SignInResult.Failed;
            // Act
            var result = await _testService.LoginUser(loginVM);
            // Assert
            Assert.False(result.Success);
            Assert.Equal("Login or password is incorrect", result.Message);
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

        [Fact]
        public void Dispose_ShouldDisposeDatabase()
        {
            // Arrange
            var repo = new Mock<IRepository<DbUser, UserDTO>>();
            var result = false;
            repo.Setup(x => x.Dispose()).Callback(() => result = true);
            // Act
            var service = new UserService(repo.Object, null, null, null, null);
            service.Dispose();
            // Assert
            Assert.True(result);
        }
    }
}
