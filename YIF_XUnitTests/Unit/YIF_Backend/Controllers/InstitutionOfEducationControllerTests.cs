using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Resources;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;

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
        public async Task GetInstitutionOfEducationsPageForAnonym_ReturnOk()
        {
            // Arrange
            var filterModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "",
                InstitutionOfEducationName = "",
                InstitutionOfEducationAbbreviation = "",
                PaymentForm = "",
                EducationForm = ""
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
            var result = await _testControl.GetInstitutionOfEducationsPageForAnonym(
                filterModel.DirectionName,
                filterModel.SpecialtyName,
                filterModel.InstitutionOfEducationName,
                filterModel.InstitutionOfEducationAbbreviation,
                filterModel.PaymentForm,
                filterModel.EducationForm,
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
                EducationForm = ""
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

            // Act
            var result = await _testControl.GetInstitutionOfEducationsPageForAnonym(
                filterModel.DirectionName,
                filterModel.SpecialtyName,
                filterModel.InstitutionOfEducationName,
                filterModel.InstitutionOfEducationAbbreviation,
                filterModel.PaymentForm,
                filterModel.EducationForm,
                1, 10);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (InstitutionOfEducationResponseApiModel)responseResult.Value;
            Assert.Equal(200, responseResult.StatusCode);
        }
    }
}
