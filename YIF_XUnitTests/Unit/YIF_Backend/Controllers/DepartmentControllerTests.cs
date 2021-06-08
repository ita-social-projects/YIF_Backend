using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Principal;
using System.Threading.Tasks;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly DepartmentController _testControl;
        private readonly Mock<IDepartmentService> _departmentService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

        public DepartmentControllerTests()
        {
            _departmentService = new Mock<IDepartmentService>();
            _testControl = new DepartmentController(_departmentService.Object);
            _httpContext = new Mock<HttpContext>();
            _fakeIdentity = new GenericIdentity("User");
            _roles = new string[] { "InstitutionOfEducationAdmin", "InstitutionOfEducationModerator" };
            _principal = new GenericPrincipal(_fakeIdentity, _roles);
            _testControl.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContext.Object
            };
        }

        [Fact]
        public async Task AddDepartment_ShouldReturnOk()
        {
            // Arrange
            _httpContext.SetupGet(x => x.User).Returns(_principal);
            var inst = new DepartmentApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>() };
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _departmentService.Setup(x => x.AddDepartment(It.IsAny<DepartmentApiModel>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.AddDepartment(inst);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddDepartment_EndpointsReturnBadRequest_IfDepartmentAlreadyExist()
        {
            // Arrange
            var inst = new DepartmentApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>() };

            // Assert
            Assert.ThrowsAsync<BadRequestException>(() => _testControl.AddDepartment(inst));
        }
    }
}
