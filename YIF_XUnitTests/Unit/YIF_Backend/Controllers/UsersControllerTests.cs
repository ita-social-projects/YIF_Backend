using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class UsersControllerTests
    {
        private static readonly Mock<IUserService<DbUser>> _userService = new Mock<IUserService<DbUser>>();
        private static readonly Mock<IJwtService> _jwtService = new Mock<IJwtService>();
        private static readonly Mock<ILogger<UsersController>> _logger = new Mock<ILogger<UsersController>>();
        private static readonly UsersController _testControl = new UsersController(_userService.Object, _jwtService.Object, _logger.Object);
        private static readonly string _guid = Guid.NewGuid().ToString("D");

        [Fact]
        public async Task GetUserAsync_EndpointReturnSuccessAndCorrectViewModel_IfUserExists()
        {
            // Arrange
            var responseModel = new ResponseModel<UserViewModel> { Success = true, Object = GetTestUsers()[0] };
            _userService.Setup(x => x.GetUserById(_guid)).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetUserAsync(_guid);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (UserViewModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Id, model.Id);
        }

        [Fact]
        public async Task GetUserAsync_EndpointReturnNotFound_IfUserNotExists()
        {
            // Arrange
            var request = Guid.NewGuid().ToString("D");
            var responseModel = new ResponseModel<UserViewModel> { Success = false, Object = null, Message = "User not found:  " + request };
            _userService.Setup(x => x.GetUserById(request)).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetUserAsync(request);
            // Assert
            var responseResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.StartsWith("User not found", responseResult.Value.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("d")]
        public async Task GetUserAsync_EndpointReturnBadRequest_IfRequestIsNotValidOrEmpty(string request)
        {
            // Act
            var result = await _testControl.GetUserAsync(request);
            // Assert
            var responseResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(responseResult.Value.ToString().Contains("The string to be parsed is null") ||
                responseResult.Value.ToString().Contains("Bad format"));
        }

        [Fact]
        public async Task GetAllUsersAsync_EndpointReturnAllUsers()
        {
            // Arrange
            var request = Guid.NewGuid().ToString("D");
            var responseModel = new ResponseModel<IEnumerable<UserViewModel>> { Success = true, Object = (IEnumerable<UserViewModel>)GetTestUsers() };
            _userService.Setup(x => x.GetAllUsers()).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetAllUsersAsync();
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (IEnumerable<UserViewModel>)responseResult.Value;
            Assert.Equal(responseModel.Object, model);
        }

        [Fact]
        public void ReturnResult_MethodReturnObjectIfStatusIsSuccess()
        {
            // Arrange
            var guid = Guid.NewGuid();
            // Act
            var privateMethod = _testControl.GetType().GetMethod("ReturnResult", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (ActionResult)privateMethod.Invoke(_testControl, new object[] { true, guid, "" });
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (Guid)responseResult.Value;
            Assert.Equal(guid, model);
        }

        [Fact]
        public void ReturnResult_MethodReturnMessageIfStatusIsFalse()
        {
            // Arrange
            var str = "Something went wrong";
            // Act
            var privateMethod = _testControl.GetType().GetMethod("ReturnResult", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (ActionResult)privateMethod.Invoke(_testControl, new object[] { false, null, str });
            // Assert
            var responseResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(str, responseResult.Value.ToString());
        }



        private List<UserViewModel> GetTestUsers()
        {
            var users = new List<UserViewModel>
            {
                new UserViewModel { Id = _guid, UserName="Kate Malash", Email = "katemalash@gmail.com"},
                new UserViewModel { Id = Guid.NewGuid().ToString("D"), UserName ="Tom Cruis", Email = "tomicocruisico@xyz.com"},
                new UserViewModel { Id = Guid.NewGuid().ToString("D"), UserName="Alice Brown", Email = "alice@bing.com"},
                new UserViewModel { Id = Guid.NewGuid().ToString("D"), UserName="Sam Samuel", Email = "sam@gmail.com"}
            };
            return users;
        }
    }
}
