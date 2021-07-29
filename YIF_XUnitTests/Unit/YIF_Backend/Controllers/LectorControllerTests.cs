using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class LectorControllerTests
    {
        private readonly Mock<ILectorService> _lectorService;
        private readonly LectorController _lectorController;
        private readonly Mock<HttpContext> _mockContext;

        public LectorControllerTests()
        {
            _lectorService = new Mock<ILectorService>();
            _lectorController = new LectorController(_lectorService.Object);
            _mockContext = new Mock<HttpContext>();
            _lectorController.ControllerContext = new ControllerContext()
            {
                HttpContext = _mockContext.Object
            };
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_EndpointReturnsOk()
        {
            var response = new ResponseApiModel<IEnumerable<DepartmentApiModel>>(new List<DepartmentApiModel>(), true);
            _lectorService.Setup(x => x.GetAllDepartments()).ReturnsAsync(response);

            // Act
            var result = await _lectorController.GetAllDepartments();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllDisciplinesAsync_EndpointReturnOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<DisciplinePostApiModel>>(new List<DisciplinePostApiModel>(), true);
            _lectorService.Setup(x => x.GetAllDisciplines()).ReturnsAsync(response);

            // Act
            var result = await _lectorController.GetAllDisciplines();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
