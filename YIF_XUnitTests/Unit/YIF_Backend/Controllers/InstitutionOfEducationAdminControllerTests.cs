using System;
using System.Collections.Generic;
using System.Resources;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class InstitutionOfEducationAdminControllerTests
    {
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IIoEAdminService> _ioEAdminService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly InstitutionOfEducationAdminController _testControl;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;
        private readonly Mock<HttpRequest> _httpRequest;

        public InstitutionOfEducationAdminControllerTests()
        {
            _ioEAdminService = new Mock<IIoEAdminService>();
            _resourceManager = new Mock<ResourceManager>();
            _testControl = new InstitutionOfEducationAdminController(_ioEAdminService.Object, _resourceManager.Object);
            _httpContext = new Mock<HttpContext>();
            _fakeIdentity = new GenericIdentity("User");
            _roles = new string[] { "InstitutionOfEducationAdmin" };
            _principal = new GenericPrincipal(_fakeIdentity, _roles);
            _testControl.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContext.Object
            };
        }
        [Fact]
        public async Task AddSpecialty_EndpointReturnsOk()
        {
            //Arrange
            _httpContext.SetupGet(x => x.User).Returns(_principal);
            _ioEAdminService.Setup(x => x.AddSpecialtyToIoe(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>()));
            //Act
            var result = await _testControl.AddSpecialtyToIoE(new SpecialtyToInstitutionOfEducationPostApiModel());
            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateSpecialtyDescription_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _ioEAdminService.Setup(x => x.UpdateSpecialtyDescription(It.IsAny<SpecialtyDescriptionUpdateApiModel>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.UpdateSpecialtyDescription(It.IsAny<SpecialtyDescriptionUpdateApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetModeratorsByUserId_ShouldReturnOk_IfEverythingIsOk()
        {
            // Arrange  
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            _ioEAdminService.Setup(x => x.GetIoEModeratorsByUserId(It.IsAny<string>()))
                .ReturnsAsync(new ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>());

            // Act
            var result = await _testControl.GetModeratorsByUserId();

            // Assert  
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteSpecialtyFromInstitutionOfEducation_ShouldReturnNoContent_IfEverythingIsOk()
        {
            // Arrange
            _ioEAdminService.Setup(x => x.DeleteSpecialtyToIoe(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>()));

            // Act
            var result = await _testControl.DeleteSpecialtyFromIoE(new SpecialtyToInstitutionOfEducationPostApiModel());

            // Assert  
            Assert.IsType<NoContentResult>(result);
        }
      
        [Fact]
        public async void GetIoEInfoByUserId_ShouldReturnOk_IfEverythingIsOk()
        {
            // Arrange  
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            _ioEAdminService.Setup(x => x.GetIoEInfoByUserId(It.IsAny<string>()))
                .ReturnsAsync(new ResponseApiModel<IoEInformationResponseApiModel>());

            // Act
            var result = await _testControl.GetIoEInfoByUserId();

            // Assert  
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetSpecialtyDescription_ShouldReturnOk_IfEverythingIsOk()
        {
            //Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            var response = new ResponseApiModel<SpecialtyToInstitutionOfEducationResponseApiModel>(new SpecialtyToInstitutionOfEducationResponseApiModel(), true);
            _ioEAdminService.Setup(x => x.GetSpecialtyToIoEDescription(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.GetSpecialtyDescription(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteIoEModerator_ShouldReturnOk_IfEverythingIsOk()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            _ioEAdminService.Setup(x => x.DeleteIoEModerator(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ResponseApiModel<DescriptionResponseApiModel>());

            // Act
            var result = await _testControl.DeleteIoEModerator(It.IsAny<string>());

            // Assert  
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(true, "success")]
        [InlineData(false, "wrong")]
        public async Task DisableIoEModerator_EndpointsReturnsResponseApiModelWithText_or_Exception(bool success, string message)
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);

            var requestId = "04b9a0c9-2545-4e28-9920-478aa6031c4b";
            var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(message), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _ioEAdminService.Setup(x => x.ChangeBannedStatusOfIoEModerator(requestId, It.IsAny<string>())).Returns(Task.FromResult(responseModel));

                // Act
                var result = await _testControl.BanIoEModerator(requestId);

                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                var model = (DescriptionResponseApiModel)responseResult.Value;
                Assert.Equal(responseModel.Object.Message, model.Message);
            }

            else
            {
                _ioEAdminService.Setup(x => x.ChangeBannedStatusOfIoEModerator(requestId, It.IsAny<string>())).Throws(error);

                // Assert
                var exсeption = await Assert.ThrowsAsync<NotFoundException>(() => _testControl.BanIoEModerator(requestId));
                Assert.Equal(error.Message, exсeption.Message);
            }
        }

        [Fact]
        public async Task AddIoELector_ShouldReturnOk()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);

            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _ioEAdminService.Setup(x => x.AddLectorToIoE(It.IsAny<string>(), It.IsAny<EmailApiModel>(), It.IsAny<HttpRequest>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.AddIoELector(It.IsAny<EmailApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddIoELector_EndpointsReturnBadRequest_IfModelStateIsNotValid(string email)
        {
            // Arrange
            var inst = new EmailApiModel() { UserEmail = email };

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _testControl.AddIoELector(inst));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddInstitutionOfEducationModerator_EndpointsReturnBadRequest_IfModelStateIsNotValid(string email)
        {
            // Arrange
            var inst = new EmailApiModel() { UserEmail = email };

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _testControl.AddIoEModerator(inst));
        }

        [Theory]
        [InlineData("moderatorEmail")]
        public async Task AddInstitutionOfEducationModerator_EndpointsReturnOk_IfEverythingIsOk(string email)
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            var inst = new EmailApiModel() { UserEmail = email };
            _ioEAdminService.Setup(x => x.AddIoEModerator(email, It.IsAny<string>(), It.IsAny<HttpRequest>())).ReturnsAsync(new ResponseApiModel<DescriptionResponseApiModel>());

            // Act
            var result = await _testControl.AddIoEModerator(inst);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddDepartment_ShouldReturnOk()
        {
            // Arrange
            var inst = new DepartmentApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>() };
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _ioEAdminService.Setup(x => x.AddDepartment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.AddDepartment(inst);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddDepartment_EndpointsReturnBadRequest_IfDepartmentAlreadyExist()
        {
            // Arrange
            var inst = new DepartmentApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>()};

            // Assert
            Assert.ThrowsAsync<BadRequestException>(() => _testControl.AddDepartment(inst));
        }
    }
}
