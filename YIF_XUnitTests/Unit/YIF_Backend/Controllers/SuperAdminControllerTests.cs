using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Resources;
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
        private readonly Mock<ResourceManager> _resourceManager;

        private readonly SuperAdminController superAdminController;
        public SuperAdminControllerTests()
        {
            _superAdminService = new Mock<ISuperAdminService>();
            _resourceManager = new Mock<ResourceManager>();

            superAdminController = new SuperAdminController(
                _superAdminService.Object,
                _resourceManager.Object);
        }

        [Theory]
        [InlineData("UniName", "email@gmailcom")]
        [InlineData("", "email@gmailcom")]
        public async Task AddUniAdmin_EndpointsReturnResponseApiModelWithJwt_IfDataСorrect(string uniName, string email)
        {
            // Arrange
            var requestModel = new InstitutionOfEducationAdminApiModel
            {
                InstitutionOfEducationName = uniName,
                Email = email
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { Success = true, Object = GetTestJwt()[0] };
            _superAdminService.Setup(x => x.AddInstitutionOfEducationAdmin(requestModel)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await superAdminController.AddInstitutionOfEducationAdmin(requestModel);
            // Assert
            var responseResult = Assert.IsType<CreatedResult>(result);
            var model = (AuthenticateResponseApiModel)responseResult.Value;
            Assert.Equal(responseModel.Object.Token, model.Token);
        }

        [Theory]
        [InlineData("NotInDatabaseUniName", "email@gmailcom")]
        public async Task AddUniAdmin_EndpointsReturnErrorNoInstitutionOfEducationWithSuchName_IfDataInСorrect(string uniName, string email)
        {
            // Arrange
            var requestModel = new InstitutionOfEducationAdminApiModel
            {
                InstitutionOfEducationName = uniName,
                Email = email
            };

            var error = new NotFoundException("ExampleErrorMessage");
            _superAdminService.Setup(x => x.AddInstitutionOfEducationAdmin(requestModel)).Throws(error);

            // Assert
            var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.AddInstitutionOfEducationAdmin(requestModel));
            Assert.Equal(error.Message, exeption.Message);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAdmin_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            superAdminController.ModelState.AddModelError("model", "error");
            // Act
            var result = await superAdminController.AddInstitutionOfEducationAdmin(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
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
        public async Task AddSchoolAdmin_EndpointsReturnErrorNoInstitutionOfEducationWithSuchName_IfDataInСorrect(string schoolName, string email, string password)
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

        [Fact]
        public async Task AddSchoolAdmin_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            superAdminController.ModelState.AddModelError("model", "error");
            // Act
            var result = await superAdminController.AddSchoolAdmin(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }

        [Theory]
        [InlineData(true, "succes")]
        [InlineData(false, "wrong")]
        public async Task DeleteInstitutionOfEducationAdmin_EndpointsReturnsResponseApiModelWithText_or_Exception(bool success, string message)
        {
            // Arrange
            var requestModel = new SchoolUniAdminDeleteApiModel { Id = "id" };
            var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(message), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _superAdminService.Setup(x => x.DeleteInstitutionOfEducationAdmin(requestModel.Id)).Returns(Task.FromResult(responseModel));
                // Act
                var result = await superAdminController.DeleteInstitutionOfEducationAdmin(requestModel.Id);
                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                var model = (DescriptionResponseApiModel)responseResult.Value;
                Assert.Equal(responseModel.Object.Message, model.Message);
            }
            else
            {
                _superAdminService.Setup(x => x.DeleteInstitutionOfEducationAdmin(requestModel.Id)).Throws(error);
                // Assert
                var exeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.DeleteInstitutionOfEducationAdmin(requestModel.Id));
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

        [Fact]
        public async Task DeleteSchoolAdmin_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            superAdminController.ModelState.AddModelError("model", "error");
            // Act
            var result = await superAdminController.DeleteSchoolAdmin(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }

        //[Theory]
        //[InlineData("UniName", "email@gmailcom", "good uni")]
        //[InlineData("", "email@gmailcom", "best uni")]
        //[InlineData("UniName", "", "")]
        //public async Task AddInstitutionOfEducation_EndpointsReturnSuccessMessage_IfDataСorrect(string name, string email, string description)
        //{
        //    // Arrange
        //    var requestModel = new InstitutionOfEducationPostApiModel
        //    {
        //        Name = name,
        //        Email = email,
        //        Description = description
        //    };

        //    var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(true, "success");
        //    _superAdminService.Setup(x => x.AddInstitutionOfEducation(requestModel)).Returns(Task.FromResult(responseModel));

        //    // Act
        //    var result = await superAdminController.AddInstitutionOfEducation(requestModel);
        //    // Assert
        //    var responseResult = Assert.IsType<CreatedResult>(result);
        //    var model = (DescriptionResponseApiModel)responseResult.Value;
        //}

        [Fact]
        public async Task AddInstitutionOfEducation_EndpointsReturnBadRequest_IfModelStateIsNotValid()
        {
            // Arrange
            superAdminController.ModelState.AddModelError("model", "error");
            // Act
            var result = await superAdminController.AddInstitutionOfEducationAndAdmin(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<DescriptionResponseApiModel>(badRequestResult.Value);
        }
        [Fact]
        public async Task GetAllUniAdminUsersAsync_EndpointReturnAllUsers()
        {
            // Arrange
            var responseModel = new ResponseApiModel<IEnumerable<InstitutionOfEducationAdminResponseApiModel>> { Success = true, 
                Object = (IEnumerable<InstitutionOfEducationAdminResponseApiModel>)new List<InstitutionOfEducationAdminResponseApiModel>
            {
                new InstitutionOfEducationAdminResponseApiModel { Id= "Id" }
            }
        };
            _superAdminService.Setup(x => x.GetAllInstitutionOfEducationAdmins()).Returns(Task.FromResult(responseModel));
            // Act
            var result = await superAdminController.GetAllUniUsersAsync();
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (IEnumerable<InstitutionOfEducationAdminResponseApiModel>)responseResult.Value;
            Assert.Equal(responseModel.Object, model);
        }

        [Theory]
        [InlineData("Адмінів немає")]
        public async Task GetAllUniAdminUsers_ReturnsNotFoundExeption(string message)
        {
            var error = new NotFoundException(message);
            _superAdminService.Setup(x => x.GetAllInstitutionOfEducationAdmins()).Throws(error);
            var result = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.GetAllUniUsersAsync());
            Assert.Equal(error.Message, result.Message);
        }

        [Fact]
        public async Task GetAllSchoolAdminUsersAsync_EndpointReturnAllUsers()
        {
            // Arrange
            var responseModel = new ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>
            {
                Success = true,
                Object = new List<SchoolAdminResponseApiModel>
            {
                new SchoolAdminResponseApiModel {Id="Id",SchoolId="Id",SchoolName="InstitutionOfEducationName"}
            }
            };
            _superAdminService.Setup(x => x.GetAllSchoolAdmins()).Returns(Task.FromResult(responseModel));
            // Act
            var result = await superAdminController.GetAllSchoolUsersAsync();
            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (IEnumerable<SchoolAdminResponseApiModel>)responseResult.Value;
            Assert.Equal(responseModel.Object, model);
        }

        [Theory]
        [InlineData("Адмінів немає")]
        public async Task GetAllSchoolAdminUsers_ReturnsNotFoundExeption(string message)
        {
            var error = new NotFoundException(message);
            _superAdminService.Setup(x => x.GetAllSchoolAdmins()).Throws(error);
            var result = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.GetAllSchoolUsersAsync());
            Assert.Equal(error.Message, result.Message);
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
