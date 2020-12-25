using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.ResultApiModels;
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

        [Theory]
        [InlineData("qtoni6@gmail.com", "QWerty-1")]
        public async Task LoginUser_EndpointsReturnLoginResponseViewModelWithJwt_IfLoginAndPasswordCorrect(string email, string password)
        {
            // Arrange
            var request = new LoginApiModel
            {
                Email = email,
                Password = password
            };

            var responseModel = new ResponseApiModel<LoginResultApiModel> { StatusCode = 200, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.LoginUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.LoginUser(request);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (LoginResultApiModel)responseResult.Value;

            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1")]
        public async Task LoginUser_EndpointReturnLoginResponseViewModelWithMessage_IfLoginOrPasswordIncorrect(string email, string password)
        {
            // Arrange
            var request = new LoginApiModel
            {
                Email = email,
                Password = password
            };

            var error = "Login or password is incorrect";
            var responseModel = new ResponseApiModel<LoginResultApiModel> { StatusCode = 400, Message = error };
            _userService.Setup(x => x.LoginUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.LoginUser(request);

            // Assert
            var responseResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = (DescriptionResultApiModel)responseResult.Value;

            Assert.Equal(error, model.Message);
        }

        [Theory]
        [InlineData("test@gmail.com", "test", "PAssword123_", "PAssword123_")]
        public async Task RegisterUser_EndpointsReturnLoginResponseViewModelWithJwt_IfDataСorrect(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var request = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var responseModel = new ResponseApiModel<LoginResultApiModel> { StatusCode = 201, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.RegisterUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.RegisterUser(request);

            // Assert
            var responseResult = Assert.IsType<CreatedResult>(result);
            var model = (LoginResultApiModel)responseResult.Value;

            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("d@gmail.com", "d", "test", "test")]
        public async Task RegisterUser_EndpointsReturnBadRequest_IfDataIncorrect(string email, string username, string password, string confirmPassword)
        {
            // Arrange
            var request = new RegisterApiModel
            {
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var error = "error message";
            var responseModel = new ResponseApiModel<LoginResultApiModel> { StatusCode = 400, Message = error };
            _userService.Setup(x => x.RegisterUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.RegisterUser(request);

            // Assert
            var responseResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = (DescriptionResultApiModel)responseResult.Value;

            Assert.Equal(error, model.Message);
        }

        private List<LoginResultApiModel> GetTestJwt()
        {
            return new List<LoginResultApiModel>
            {
                new LoginResultApiModel { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmZWY1ZmM4LTA1NjEtNDI2OS04Zjc1LTk1N2RhNzg4ODkyOCIsImVtYWlsIjoicXRvbmk2QGdtYWlsLmNvbSIsIm5hbWUiOiJBcm5vbGRCZWFzbGV5Iiwicm9sZXMiOiJVbml2ZXJzaXR5QWRtaW4iLCJleHAiOjE2MDg1MDQxMjl9.araGavMMEaMXF2fjFU_OH72ipfJuae21vzxEcfTp_L0" },
                new LoginResultApiModel { Token = null },
            };
        }
    }
}
