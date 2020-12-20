using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF.Core.Domain.ViewModels.UserViewModels;
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
            var request = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            var responseModel = new ResponseModel<LoginResponseViewModel> { Success = true, Object = GetTestJwt()[0] };
            _userService.Setup(x => x.LoginUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.LoginUser(request);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (ResponseModel<LoginResponseViewModel>)responseResult.Value;            

            Assert.Equal(responseModel.Object.UserToken, model.Object.UserToken);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1")]
        [InlineData("qtoni6@gmail.com", "d")]
        [InlineData("", "")]
        public async Task LoginUser_EndpointReturnLoginResponseViewModelWithoutJwt_IfLoginOrPasswordInorrect(string email, string password)
        {
            // Arrange
            var request = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            var responseModel = new ResponseModel<LoginResponseViewModel> { Success = false, Object = GetTestJwt()[1] };
            _userService.Setup(x => x.LoginUser(request)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await _testControl.LoginUser(request);

            // Assert
            var responseResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = (ResponseModel<LoginResponseViewModel>)responseResult.Value;

            Assert.Null(model.Object.UserToken);
        }

        private List<LoginResponseViewModel> GetTestJwt()
        {
            return new List<LoginResponseViewModel>
            {
                new LoginResponseViewModel { UserToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmZWY1ZmM4LTA1NjEtNDI2OS04Zjc1LTk1N2RhNzg4ODkyOCIsImVtYWlsIjoicXRvbmk2QGdtYWlsLmNvbSIsIm5hbWUiOiJBcm5vbGRCZWFzbGV5Iiwicm9sZXMiOiJVbml2ZXJzaXR5QWRtaW4iLCJleHAiOjE2MDg1MDQxMjl9.araGavMMEaMXF2fjFU_OH72ipfJuae21vzxEcfTp_L0" },
                new LoginResponseViewModel { UserToken = null }
            };
        }
    }
}
