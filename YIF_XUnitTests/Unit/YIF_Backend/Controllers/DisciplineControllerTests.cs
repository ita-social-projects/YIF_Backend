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
    public class DisciplineControllerTests
    {
        private readonly DisciplineController _testControl;
        private readonly Mock<IDisciplineService> _disciplineService;
        private readonly Mock<HttpContext> _httpContext;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

        public DisciplineControllerTests()
        {
            _disciplineService = new Mock<IDisciplineService>();
            _testControl = new DisciplineController(_disciplineService.Object);
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
        public async Task AddDiscipline_ShouldReturnOk()
        {
            // Arrange
            _httpContext.SetupGet(x => x.User).Returns(_principal);
            var discipline = new DisciplineApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>() };
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _disciplineService.Setup(x => x.AddDiscipline(It.IsAny<DisciplineApiModel>())).ReturnsAsync(response);

            //Act
            var result = await _testControl.AddDiscipline(discipline);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddDiscipline_EndpointsReturnBadRequest_IfDisciplineAlreadyExist()
        {
            // Arrange
            var discipline = new DisciplineApiModel() { Name = It.IsAny<string>(), Description = It.IsAny<string>() };

            // Assert
            Assert.ThrowsAsync<BadRequestException>(() => _testControl.AddDiscipline(discipline));
        }
    }
}
