using System.Resources;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class InstitutionOfEducationModeratorControllerTests
    {
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IIoEModeratorService> _ioEModeratorService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly InstitutionOfEducationModeratorController _testControl;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

        public InstitutionOfEducationModeratorControllerTests()
        {
            _ioEModeratorService = new Mock<IIoEModeratorService>();
            _resourceManager = new Mock<ResourceManager>();
            _testControl = new InstitutionOfEducationModeratorController(_ioEModeratorService.Object, _resourceManager.Object);
            _httpContext = new Mock<HttpContext>();
            _fakeIdentity = new GenericIdentity("User");
            _roles = new string[] { "InstitutionOfEducationModerator" };
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
            _ioEModeratorService.Setup(x => x.AddSpecialtyToIoe(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>()));
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
            _ioEModeratorService.Setup(x => x.UpdateSpecialtyDescription(It.IsAny<SpecialtyDescriptionUpdateApiModel>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.UpdateSpecialtyDescription(It.IsAny<SpecialtyDescriptionUpdateApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteSpecialtyFromInstitutionOfEducation_ShouldReturnNoContent_IfEverythingIsOk()
        {
            // Arrange
            _ioEModeratorService.Setup(x => x.DeleteSpecialtyToIoe(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>()));

            // Act
            var result = await _testControl.DeleteSpecialtyFromIoE(new SpecialtyToInstitutionOfEducationPostApiModel());

            // Assert  
            Assert.IsType<NoContentResult>(result);
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
            _ioEModeratorService.Setup(x => x.GetSpecialtyToIoEDescription(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.GetSpecialtyDescription(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAdminByUserId_EndpointReturnsOk()
        {
            //Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            var response = new ResponseApiModel<IoEAdminForIoEModeratorResponseApiModel>(new IoEAdminForIoEModeratorResponseApiModel(), true);

            _ioEModeratorService.Setup(x => x.GetIoEAdminByUserId(It.IsAny<string>())).ReturnsAsync(response);
            var result = await _testControl.GetAdminByUserId();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetIoEInfoByUserId_ShouldReturnSuccess()
        {
            // Arrange  
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            _ioEModeratorService.Setup(x => x.GetIoEInfoByUserId(It.IsAny<string>()))
                .ReturnsAsync(new ResponseApiModel<IoEInformationResponseApiModel>());

            // Act
            var result = await _testControl.GetIoEInfoByUserId();

            // Assert  
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
