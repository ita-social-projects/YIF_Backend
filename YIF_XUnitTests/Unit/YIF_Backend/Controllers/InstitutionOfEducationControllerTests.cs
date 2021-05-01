using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class InstitutionOfEducationControllerTests
    {
        private readonly Mock<IInstitutionOfEducationService<InstitutionOfEducation>> _institutionOfEducationService = new Mock<IInstitutionOfEducationService<InstitutionOfEducation>>();
        private readonly Mock<ILogger<InstitutionOfEducationController>> _logger;
        private readonly InstitutionOfEducationController _testControl;

        public InstitutionOfEducationControllerTests()
        {
            _institutionOfEducationService = new Mock<IInstitutionOfEducationService<InstitutionOfEducation>>();
            _logger = new Mock<ILogger<InstitutionOfEducationController>>();
            _testControl = new InstitutionOfEducationController(_institutionOfEducationService.Object);
        }
        [Fact]
        public async Task GetInstitutionOfEducationsPageForAnonymous_ReturnOk()
        {
            // Arrange
            var filterModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "",
                InstitutionOfEducationName = "",
                InstitutionOfEducationAbbreviation = "",
                PaymentForm = "",
                EducationForm = "",
                InstitutionOfEducationType = ""
            };
            var pageModel = new PageApiModel
            {
                Page = 1,
                PageSize = 10,
                Url = "link"
            };
            var _iOEs = new PageResponseApiModel<InstitutionsOfEducationResponseApiModel>
            {
                ResponseList = new List<InstitutionsOfEducationResponseApiModel>
                {
                    new InstitutionsOfEducationResponseApiModel()
                    {
                        Id = "1"
                    }
                }
            };
            _institutionOfEducationService.Setup(x => x.GetInstitutionOfEducationsPage(filterModel, pageModel)).Returns(Task.FromResult(_iOEs));

            // Act
            var result = await _testControl.GetInstitutionOfEducationsPageForAnonymous(
                filterModel.DirectionName,
                filterModel.SpecialtyName,
                filterModel.InstitutionOfEducationName,
                filterModel.InstitutionOfEducationAbbreviation,
                filterModel.PaymentForm,
                filterModel.EducationForm,
                filterModel.InstitutionOfEducationType,
                1, 10);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (InstitutionOfEducationResponseApiModel)responseResult.Value;
            Assert.Equal(200, responseResult.StatusCode);
        }

        [Fact]
        public async Task GetInstitutionOfEducationsPageForAuthorizedUser_ReturnOk()
        {
            // Arrange
            var filterModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "",
                InstitutionOfEducationName = "",
                InstitutionOfEducationAbbreviation = "",
                PaymentForm = "",
                EducationForm = "",
                InstitutionOfEducationType = ""
            };
            var pageModel = new PageApiModel
            {
                Page = 1,
                PageSize = 10,
                Url = "link"
            };
            var _iOEs = new PageResponseApiModel<InstitutionsOfEducationResponseApiModel>
            {
                ResponseList = new List<InstitutionsOfEducationResponseApiModel>
                {
                    new InstitutionsOfEducationResponseApiModel()
                    {
                        Id = "1"
                    }
                }
            };
            _institutionOfEducationService.Setup(x => x.GetInstitutionOfEducationsPageForUser(filterModel, pageModel, "1")).Returns(Task.FromResult(_iOEs));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1"),
            }, "mock"));

            var controller = new InstitutionOfEducationController(_institutionOfEducationService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };


            // Act
            var result = await controller.GetInstitutionOfEducationsPageForAuthorizedUser(
                filterModel.DirectionName,
                filterModel.SpecialtyName,
                filterModel.InstitutionOfEducationName,
                filterModel.InstitutionOfEducationAbbreviation,
                filterModel.PaymentForm,
                filterModel.EducationForm,
                filterModel.InstitutionOfEducationType,
                1, 10);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (InstitutionOfEducationResponseApiModel)responseResult.Value;
            Assert.Equal(200, responseResult.StatusCode);
        }
    }
}
