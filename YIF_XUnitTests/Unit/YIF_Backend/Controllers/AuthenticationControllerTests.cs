using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IUserService<DbUser>> _userService;
        private readonly AuthenticationController _testControl;

        public AuthenticationControllerTests()
        {
            _userService = new Mock<IUserService<DbUser>>();
            _testControl = new AuthenticationController(_userService.Object);
        }

        [Fact]
        public async Task LoginUser_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            _testControl.ModelState.AddModelError("model", "error");
            // Act
            var result = await _testControl.LoginUser(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }

        [Theory]
        [InlineData("qtoni6@gmail.com", "QWerty-1")]
        public async Task LoginUser_EndpointsReturnResponseApiModelWithJwt_IfLoginAndPasswordCorrect(string email, string password)
        {
            // Arrange
            var request = new LoginApiModel
            {
                Email = email,
                Password = password
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.LoginUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.LoginUser(request);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1")]
        public async Task LoginUser_EndpointReturnResponseApiodelWithMessage_IfLoginOrPasswordIncorrect(string email, string password)
        {
            // Arrange
            var request = new LoginApiModel
            {
                Email = email,
                Password = password,
            };

            var error = "Login or password is incorrect";
            _userService.Setup(x => x.LoginUser(request)).Throws(new BadRequestException(error));

            // Assert
            var exeption = await Assert.ThrowsAsync<BadRequestException>(() => _testControl.LoginUser(request));
            Assert.Equal(error, exeption.Message);
        }

        [Fact]
        public async Task RegisterUser_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            _testControl.ModelState.AddModelError("model", "error");
            // Act
            var result = await _testControl.RegisterUser(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }

        [Theory]
        [InlineData("test@gmail.com", "test", "PAssword123_", "PAssword123_")]
        public async Task RegisterUser_EndpointsReturnResponseApiModelWithJwt_IfDataСorrect(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var request = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.RegisterUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.RegisterUser(request);

            // Assert
            var responseResult = Assert.IsType<CreatedResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Fact]
        public async Task RegisterUser_EndpointsReturnBadRequest_IfDataIncorrect()
        {
            // Arrange
            var request = new RegisterApiModel();
            var error = new InvalidOperationException("error message");
            _userService.Setup(x => x.RegisterUser(request)).Throws(error);

            // Assert
            var exeption = await Assert.ThrowsAsync<InvalidOperationException>(() => _testControl.RegisterUser(request));
            Assert.Equal(error.Message, exeption.Message);
        }

        [Fact]
        public async Task RegisterUser_EndpointsReturnBadRequest_IfEmailAlreadyExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            //httpContext.Request.Scheme = "scheme";
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            var testControl = new AuthenticationController(_userService.Object) { ControllerContext = controllerContext };

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("url").Verifiable();
            testControl.Url = mockUrlHelper.Object;

            var request = new RegisterApiModel
            {
                Email = "d@gmail.com",
                Username = "d",
                Password = "test",
                ConfirmPassword = "test"
            };
            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel>(false, "message");
            _userService.Setup(x => x.RegisterUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await testControl.RegisterUser(request);

            // Assert
            var responseResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<RedirectResponseApiModel>(responseResult.Value);
        }

        [Fact]
        public async Task Refresh_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            _testControl.ModelState.AddModelError("model", "error");
            // Act
            var result = await _testControl.Refresh(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }

        [Fact]
        public async Task Refresh_EndpointsReturnsNewJwt_ifDataСorrect()
        {
            // Arrange
            var request = new TokenRequestApiModel
            {
                Token = "token",
                RefreshToken = "refreshToken"
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.RefreshToken(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.Refresh(request);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Fact]
        public async Task Refresh_EndpointsReturnsException_ifDataIncorrect()
        {
            // Arrange
            var request = new TokenRequestApiModel
            {
                Token = "token",
                RefreshToken = "refreshToken"
            };
            var error = new BadRequestException("error message");
            _userService.Setup(x => x.RefreshToken(request)).Throws(error);

            // Assert
            var exeption = await Assert.ThrowsAsync<BadRequestException>(() => _testControl.Refresh(request));
            Assert.Equal(error.Message, exeption.Message);
        }

        private List<AuthenticateResponseApiModel> GetTestJwt()
        {
            return new List<AuthenticateResponseApiModel>
            {
                new AuthenticateResponseApiModel { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmZWY1ZmM4LTA1NjEtNDI2OS04Zjc1LTk1N2RhNzg4ODkyOCIsImVtYWlsIjoicXRvbmk2QGdtYWlsLmNvbSIsIm5hbWUiOiJBcm5vbGRCZWFzbGV5Iiwicm9sZXMiOiJVbml2ZXJzaXR5QWRtaW4iLCJleHAiOjE2MDg1MDQxMjl9.araGavMMEaMXF2fjFU_OH72ipfJuae21vzxEcfTp_L0" },
                new AuthenticateResponseApiModel { Token = null },
            };
        }
    }
}
