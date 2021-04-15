using System.Collections.Generic;
using System.Resources;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
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
    public class InstitutionOfEducationAdminControllerTests
    {
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IIoEAdminService> _ioEAdminService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly InstitutionOfEducationAdminController _testControl;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

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
    }
}
