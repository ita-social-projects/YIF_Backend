using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class SpecialtyControllerTests
    {
        private readonly Mock<ISpecialityService> _specialtyService;
        private readonly Mock<ILogger<SpecialtyController>> _logger;
        private readonly SpecialtyController _testControl;

        public SpecialtyControllerTests()
        {
            _specialtyService = new Mock<ISpecialityService>();
            _logger = new Mock<ILogger<SpecialtyController>>();
            _testControl = new SpecialtyController(_specialtyService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllSpecialtiesAsync_EndpointReturnsOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>
            {
                Object = new List<SpecialtyResponseApiModel>().AsEnumerable()
            };
            _specialtyService.Setup(x => x.GetAllSpecialties()).ReturnsAsync(response);
            // Act
            var result = await _testControl.GetAllSpecialtiesAsync();
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SpecialtyResponseApiModel>>(responseResult.Value);
        }

        [Theory]
        [InlineData("direction", "speciality", "universityName", "universityAbbreviation")]
        [InlineData("", "speciality", "", "universityAbbreviation")]
        [InlineData("", "", "", "")]
        public async Task GetAllSpecialtiesNamesAsync_EndpointReturnsOk(
            string directionName,
            string specialityName,
            string universityName,
            string universityAbbreviation)
        {
            // Arrange

            var filterModel = new FilterApiModel
            {
                DirectionName = directionName,
                SpecialityName = specialityName,
                UniversityName = universityName,
                UniversityAbbreviation = universityAbbreviation
            };

            var response = new List<string>().AsEnumerable();
            _specialtyService.Setup(x => x.GetSpecialtiesNamesByFilter(It.IsAny<FilterApiModel>())).ReturnsAsync(response);
            // Act
            var result = await _testControl.GetAllSpecialtiesNamesAsync(directionName, specialityName, universityName, universityAbbreviation);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<string>>(responseResult.Value);
        }

        [Theory]
        [InlineData("34cc87d9-6d76-44ac-9dda-15852feb9e72")]
        [InlineData("34cc87d96d7644ac9dda15852feb9e72")]
        public async Task GetSpecialtyAsync_EndpointReturnsOk(string id)
        {
            // Arrange
            var response = new ResponseApiModel<SpecialtyResponseApiModel>(new SpecialtyResponseApiModel(), true);
            _specialtyService.Setup(x => x.GetSpecialtyById(It.IsAny<string>())).ReturnsAsync(response);
            // Act
            var result = await _testControl.GetSpecialtyAsync(id);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<SpecialtyResponseApiModel>(responseResult.Value);
        }
    }
}
