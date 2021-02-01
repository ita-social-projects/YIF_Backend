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
    public class DirectionControllerTests
    {
        private readonly Mock<IDirectionService> _directionService;
        private readonly DirectionController _directionController;

        public DirectionControllerTests()
        {
            _directionService = new Mock<IDirectionService>();
            _directionController = new DirectionController(_directionService.Object);
        }

        [Fact]
        public async void GetAllDirectionsTestAsync()
        {
            // Arrange
            var responseModel = new PageResponseApiModel<DirectionResponseApiModel>
            {
                CurrentPage = 1,
                PrevPage = "prevlink",
                PageSize = 10,
                NextPage = "nextlink",
                ResponseList = new List<DirectionResponseApiModel> {
                    new DirectionResponseApiModel {Id = "testId1", Name = "nameId1" },
                    new DirectionResponseApiModel {Id = "testId2", Name = "nameId2"}
                },
                TotalPages = 1
            };

            _directionService.Setup(x => x.GetAllDirections(new PageApiModel { Page = 1, PageSize = 10, Url = "link" }))
                .Returns(Task.FromResult(responseModel));

            // Act
            var result = await _directionController.GetAllDirections();

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, responseResult.StatusCode);
        }

        [Fact]
        public async void GetDirectionNamesTestAsync()
        {
            IEnumerable<string> responseModel = new List<string> { "SomeDirection" };

            var filter = new FilterApiModel
            {
                DirectionName = "SomeDirection",
                SpecialityName = "SomeSpeciality",
                UniversityName = "SomeUniversity",
                UniversityAbbreviation = "SU"
            };

            _directionService.Setup(x => x.GetDirectionsNamesByFilter(filter))
                .Returns(Task.FromResult(responseModel));

            // Act
            var result = await _directionController.GetDirectionNames(filter.DirectionName, filter.SpecialityName, filter.UniversityName, filter.UniversityAbbreviation);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, responseResult.StatusCode);
        }
    }
}
