using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class UsersControllerTests
    {
        private static readonly Mock<IUserService<DbUser>> _userService = new Mock<IUserService<DbUser>>();
        private static readonly Mock<ILogger<UsersController>> _logger = new Mock<ILogger<UsersController>>();
        private static readonly UsersController _testControl = new UsersController(_userService.Object, _logger.Object);
        private static readonly string _guid = Guid.NewGuid().ToString("D");

        [Fact]
        public async Task GetUserAsync_EndpointReturnSuccessAndCorrectViewModel_IfUserExists()
        {
            // Arrange
            var responseModel = new ResponseApiModel<UserApiModel> { StatusCode = 200, Object = GetTestUsers()[0] };
            _userService.Setup(x => x.GetUserById(_guid)).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetUserAsync(_guid);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (UserApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Id, model.Id);
        }

        [Fact]
        public async Task GetUserAsync_EndpointReturnNotFound_IfUserNotExists()
        {
            // Arrange
            var request = Guid.NewGuid().ToString("D");
            var responseModel = new ResponseApiModel<UserApiModel> { StatusCode = 404, Object = null, Message = "User not found:  " + request };
            _userService.Setup(x => x.GetUserById(request)).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetUserAsync(request);
            // Assert
            var responseResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.StartsWith("User not found", ((DescriptionResponseApiModel)responseResult.Value).Message);
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
            Assert.True(((DescriptionResponseApiModel)responseResult.Value).Message.Contains("The string to be parsed is null") ||
                ((DescriptionResponseApiModel)responseResult.Value).Message.Contains("Bad format"));
        }

        [Fact]
        public async Task GetAllUsersAsync_EndpointReturnAllUsers()
        {
            // Arrange
            var request = Guid.NewGuid().ToString("D");
            var responseModel = new ResponseApiModel<IEnumerable<UserApiModel>> { StatusCode = 200, Object = (IEnumerable<UserApiModel>)GetTestUsers() };
            _userService.Setup(x => x.GetAllUsers()).Returns(Task.FromResult(responseModel));
            // Act
            var result = await _testControl.GetAllUsersAsync();
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (IEnumerable<UserApiModel>)responseResult.Value;
            Assert.Equal(responseModel.Object, model);
        }



        private List<UserApiModel> GetTestUsers()
        {
            var users = new List<UserApiModel>
            {
                new UserApiModel { Id = _guid, UserName="Kate Malash", Email = "katemalash@gmail.com"},
                new UserApiModel { Id = Guid.NewGuid().ToString("D"), UserName ="Tom Cruis", Email = "tomicocruisico@xyz.com"},
                new UserApiModel { Id = Guid.NewGuid().ToString("D"), UserName="Alice Brown", Email = "alice@bing.com"},
                new UserApiModel { Id = Guid.NewGuid().ToString("D"), UserName="Sam Samuel", Email = "sam@gmail.com"}
            };
            return users;
        }
    }
}
