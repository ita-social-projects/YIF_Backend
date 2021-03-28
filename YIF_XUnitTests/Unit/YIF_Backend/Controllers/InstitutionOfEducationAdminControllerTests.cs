using System;
using System.Collections.Generic;
using System.Resources;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class InstitutionOfEducationAdminControllerTests
    {
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IIoEAdminService> _ioeService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly InstitutionOfEducationAdminController _testControl;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

        public InstitutionOfEducationAdminControllerTests()
        {
            _ioeService = new Mock<IIoEAdminService>();
            _resourceManager = new Mock<ResourceManager>();
            _testControl = new InstitutionOfEducationAdminController(_ioeService.Object, _resourceManager.Object);
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
            _ioeService.Setup(x => x.AddSpecialtyToIoe(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>()));
            //Act
            var result = await _testControl.AddSpecialtyToIoE(new SpecialtyToInstitutionOfEducationPostApiModel());
            //Assert
            Assert.IsType<OkResult>(result);
        }

    }
}
