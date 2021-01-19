using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class SuperAdminControllerTests
    {
        private readonly Mock<ISuperAdminService> _superAdminService;
        private readonly SuperAdminController superAdminController;
        private readonly Mock<ILogger<SuperAdminController>> _logger;
        public SuperAdminControllerTests()
        {
            _superAdminService = new Mock<ISuperAdminService>();
            _logger = new Mock<ILogger<SuperAdminController>>();
            superAdminController = new SuperAdminController(_superAdminService.Object,
                                                            _logger.Object);

        }
        [Theory]
        [InlineData("UniName", "email@gmailcom", "Password1+")]
        [InlineData("", "email@gmailcom", "Password1+")]
        public async Task AddUniAdmin_EndpointsReturnResponseApiModelWithJwt_IfDataСorrect(string uniName, string email, string password)
        {
            // Arrange
            var requestModel = new UniversityAdminApiModel
            {
                UniversityName = uniName,
                Email = email,
                Password = password
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _superAdminService.Setup(x => x.AddUniversityAdmin(requestModel)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await superAdminController.AddUniversityAdmin(requestModel);
            // Assert
            var responseResult = Assert.IsType<CreatedResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("NotInDatabaseUniName", "email@gmailcom", "Password1+")]
        public async Task AddUniAdmin_EndpointsReturnErrorNoUniversityWithSuchName_IfDataInСorrect(string uniName, string email, string password)
        {
            // Arrange
            var requestModel = new UniversityAdminApiModel
            {
                UniversityName = uniName,
                Email = email,
                Password = password
            };

            var error = new NotFoundException("ExampleErrorMessage");
            _superAdminService.Setup(x => x.AddUniversityAdmin(requestModel)).Throws(error);

            // Assert
            var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.AddUniversityAdmin(requestModel));
            Assert.Equal(error.Message, exeption.Message);
        }




        [Theory]
        [InlineData("SchoolName", "email@gmailcom", "Password1+")]
        public async Task AddSchoolAdmin_EndpointsReturnResponseApiModelWithJwt_IfDataСorrect(string schoolName, string email, string password)
        {
            // Arrange
            var requestModel = new SchoolAdminApiModel
            {
                SchoolName = schoolName,
                Email = email,
                Password = password
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _superAdminService.Setup(x => x.AddSchoolAdmin(requestModel)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await superAdminController.AddSchoolAdmin(requestModel);
            // Assert
            var responseResult = Assert.IsType<CreatedResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("NotInDatabaseSchoolName", "email@gmailcom", "Password1+")]
        public async Task AddSchoolAdmin_EndpointsReturnErrorNoUniversityWithSuchName_IfDataInСorrect(string schoolName, string email, string password)
        {
            // Arrange
            var requestModel = new SchoolAdminApiModel
            {
                SchoolName = schoolName,
                Email = email,
                Password = password
            };

            var error = new NotFoundException("ExampleErrorMessage");
            _superAdminService.Setup(x => x.AddSchoolAdmin(requestModel)).Throws(error);

            // Assert
            var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.AddSchoolAdmin(requestModel));
            Assert.Equal(error.Message, exeption.Message);
        }


        [Theory]
        [InlineData(true, "succes")]
        [InlineData(false, "wrong")]
        public async Task DeleteUniversityAdmin_EndpointsReturnsResponseApiModelWithText_or_Exception(bool success, string message)
        {
            // Arrange
            var requestModel = new SchoolUniAdminDeleteApiModel { Id = "id" };
            var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(message), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _superAdminService.Setup(x => x.DeleteUniversityAdmin(requestModel)).Returns(Task.FromResult(responseModel));
                // Act
                var result = await superAdminController.DeleteUniversityAdmin(requestModel);
                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                var model = (DescriptionResponseApiModel)responseResult.Value;
                Assert.Equal(responseModel.Object.Message, model.Message);
            }
            else
            {
                _superAdminService.Setup(x => x.DeleteUniversityAdmin(requestModel)).Throws(error);
                // Assert
                var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.DeleteUniversityAdmin(requestModel));
                Assert.Equal(error.Message, exeption.Message);
            }
        }
        [Theory]
        [InlineData(true, "succes")]
        [InlineData(false, "wrong")]
        public async Task DeleteSchoolAdmin_EndpointsReturnsResponseApiModelWithText_or_Exception(bool success, string message)
        {
            // Arrange
            var requestModel = new SchoolUniAdminDeleteApiModel { Id = "id" };
            var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(message), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _superAdminService.Setup(x => x.DeleteSchoolAdmin(requestModel)).Returns(Task.FromResult(responseModel));
                // Act
                var result = await superAdminController.DeleteSchoolAdmin(requestModel);
                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                var model = (DescriptionResponseApiModel)responseResult.Value;
                Assert.Equal(responseModel.Object.Message, model.Message);
            }
            else
            {
                _superAdminService.Setup(x => x.DeleteSchoolAdmin(requestModel)).Throws(error);
                // Assert
                var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.DeleteSchoolAdmin(requestModel));
                Assert.Equal(error.Message, exeption.Message);
            }
        }


        private List<AuthenticateResponseApiModel> GetTestJwt()
        {
            return new List<AuthenticateResponseApiModel>
            {
                new AuthenticateResponseApiModel { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmZWY1ZmM4LTA1NjEtNDI2OS04Zjc1LTk1N2RhNzg4ODkyOCIsImVtYWlsIjoicXRvbmk2QGdtYWlsLmNvbSIsIm5hbWUiOiJBcm5vbGRCZWFzbGV5Iiwicm9sZXMiOiJVbml2ZXJzaXR5QWRtaW4iLCJleHAiOjE2MDg1MDQxMjl9.araGavMMEaMXF2fjFU_OH72ipfJuae21vzxEcfTp_L0" },
                new AuthenticateResponseApiModel { Token = null },
            };
        }
    }
}
