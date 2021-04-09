using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        private readonly Mock<ISpecialtyService> _specialtyService;
        private readonly Mock<ILogger<SpecialtyController>> _logger;
        private readonly SpecialtyController _testControl;
        private readonly Mock<HttpContext> _mockContext;
        private readonly GenericIdentity _fakeIdentity;
        private readonly string[] _roles;
        private readonly GenericPrincipal _principal;

        public SpecialtyControllerTests()
        {
            _specialtyService = new Mock<ISpecialtyService>();
            _logger = new Mock<ILogger<SpecialtyController>>();
            _testControl = new SpecialtyController(_specialtyService.Object, _logger.Object);
            _mockContext = new Mock<HttpContext>();
            _fakeIdentity = new GenericIdentity("User");
            _roles = new string[] {"Graduate"};
            _principal = new GenericPrincipal(_fakeIdentity, _roles);
            _testControl.ControllerContext = new ControllerContext()
            {
                HttpContext = _mockContext.Object
            };
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

            var apiModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "",
                InstitutionOfEducationName = "",
                InstitutionOfEducationAbbreviation = "",
                PaymentForm = "",
                EducationForm = ""
            };
            // Act
            var result = await _testControl.GetAllSpecialtiesAsync(apiModel.DirectionName, apiModel.SpecialtyName,
                apiModel.InstitutionOfEducationName,
                apiModel.InstitutionOfEducationAbbreviation, apiModel.PaymentForm, apiModel.EducationForm);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SpecialtyResponseApiModel>>(responseResult.Value);
        }

        [Fact]
        public async Task GetAllSpecialtiesAsyncForAuthorizedUser_EndpointReturnsOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>
            {
                Object = new List<SpecialtyResponseApiModel>().AsEnumerable()
            };
            var apiModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "",
                InstitutionOfEducationName = "",
                InstitutionOfEducationAbbreviation = "",
                PaymentForm = "",
                EducationForm = ""
            };
            _specialtyService.Setup(x => x.GetAllSpecialtiesByFilterForUser(apiModel, It.IsAny<string>())).Returns(Task.FromResult(response.Object));
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim("id", "1"),
           }, "mock"));

            var controller = new SpecialtyController(_specialtyService.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act
            var result = await controller.GetAllSpecialtiesAsyncForAuthorizedUser(apiModel.DirectionName, apiModel.SpecialtyName,
                apiModel.InstitutionOfEducationName,
                apiModel.InstitutionOfEducationAbbreviation, apiModel.PaymentForm, apiModel.EducationForm);
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SpecialtyResponseApiModel>>(responseResult.Value);
        }
        [Theory]
        [InlineData("direction", "speciality", "institutionOfEducationName", "institutionOfEducationAbbreviation")]
        [InlineData("", "speciality", "", "institutionOfEducationAbbreviation")]
        [InlineData("", "", "", "")]
        public async Task GetAllSpecialtiesNamesAsync_EndpointReturnsOk(
            string directionName,
            string specialityName,
            string institutionOfEducationName,
            string institutionOfEducationAbbreviation)
        {
            // Arrange

            var filterModel = new FilterApiModel
            {
                DirectionName = directionName,
                SpecialtyName = specialityName,
                InstitutionOfEducationName = institutionOfEducationName,
                InstitutionOfEducationAbbreviation = institutionOfEducationAbbreviation
            };

            var response = new List<string>().AsEnumerable();
            _specialtyService.Setup(x => x.GetSpecialtiesNamesByFilter(It.IsAny<FilterApiModel>()))
                .ReturnsAsync(response);
            // Act
            var result = await _testControl.GetAllSpecialtiesNamesAsync(directionName, specialityName,
                institutionOfEducationName, institutionOfEducationAbbreviation);
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

        [Fact]
        public async Task GetSpecialtyDescriptionsAsync_EndpointReturnsOk()
        {
            // Arrange
            var list = new List<SpecialtyToInstitutionOfEducationResponseApiModel>()
                {new SpecialtyToInstitutionOfEducationResponseApiModel()}.AsEnumerable();
            _specialtyService.Setup(x => x.GetAllSpecialtyDescriptionsById(It.IsAny<string>())).ReturnsAsync(list);
            // Act
            var result = await _testControl.GetSpecialtyDescriptionsAsync(It.IsAny<string>());
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>>(
                responseResult.Value);
        }

        [Fact]
        public async Task AddSpecialtyAndUniversityToFavorite_EndpointReturnsOk()
        {
            // Arrange
            _mockContext.SetupGet(hc => hc.User).Returns(_principal);
            _mockContext.SetupGet(x => x.Request.Scheme).Returns(It.IsAny<string>());
            _mockContext.SetupGet(x => x.Request.Host).Returns(It.IsAny<HostString>());
            _mockContext.SetupGet(x => x.Request.Path).Returns(It.IsAny<string>());
            _specialtyService.Setup(x =>
                x.AddSpecialtyAndInstitutionOfEducationToFavorite(It.IsAny<SpecialtyAndInstitutionOfEducationToFavoritePostApiModel>(),
                    It.IsAny<string>()));

            // Act
            var result = await _testControl.AddSpecialtyAndInstitutionOfEducationToFavorite(
                new SpecialtyAndInstitutionOfEducationToFavoritePostApiModel()
                {
                    SpecialtyId = It.IsAny<string>(),
                    InstitutionOfEducationId = It.IsAny<string>()
                });

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task DeleteSpecialtyAndUniversityFromFavorite_EndpointReturnsNoContentResult()
        {
            // Arrange
            _mockContext.SetupGet(hc => hc.User).Returns(_principal);
            _specialtyService.Setup(x =>
                x.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()));

            // Act
            var result =
                await _testControl.RemoveSpecialtyAndInstitutionOfEducationFromFavorite(It.IsAny<string>(),
                    It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddSpecialtyToFavorite_EndpointReturnsOk()
        {
            // Arrange
            _mockContext.SetupGet(hc => hc.User).Returns(_principal);
            _mockContext.SetupGet(x => x.Request.Scheme).Returns(It.IsAny<string>());
            _mockContext.SetupGet(x => x.Request.Host).Returns(It.IsAny<HostString>());
            _mockContext.SetupGet(x => x.Request.Path).Returns(It.IsAny<string>());
            _specialtyService.Setup(x => x.AddSpecialtyToFavorite(It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = await _testControl.AddSpecialtyToFavorite("Id");

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task DeleteSpecialtyToFavorite_EndpointReturnsNoContentResult()
        {
            // Arrange
            _mockContext.SetupGet(hc => hc.User).Returns(_principal);
            _specialtyService.Setup(x => x.DeleteSpecialtyFromFavorite(It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var result = await _testControl.DeleteSpecialtyFromFavorite(It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllExamsNames_EndpointReturnsOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<ExamsResponseApiModel>>(new List<ExamsResponseApiModel>(), true);
            _specialtyService.Setup(x => x.GetExams()).ReturnsAsync(response);

            // Act
            var result = await _testControl.GetAllExamsNames();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllEducationFormsNames_EndpointReturnsOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<string>>(new List<string>(), true);
            _specialtyService.Setup(x => x.GetEducationForms()).ReturnsAsync(response);

            // Act
            var result = await _testControl.GetAllEducationFormsNames();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllPaymentFormsNames_EndpointReturnsOk()
        {
            // Arrange
            var response = new ResponseApiModel<IEnumerable<string>>(new List<string>(), true);
            _specialtyService.Setup(x => x.GetPaymentForms()).ReturnsAsync(response);

            // Act
            var result = await _testControl.GetAllPaymentFormsNames();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
