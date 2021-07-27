using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;
using YIF_XUnitTests.Unit.TestData;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    public class SuperAdminControllerTests
    {
        private readonly Mock<ISuperAdminService> _superAdminService;
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly HttpContext _httpContext;

        private readonly SuperAdminController superAdminController;
        public SuperAdminControllerTests()
        {
            _superAdminService = new Mock<ISuperAdminService>();
            _resourceManager = new Mock<ResourceManager>();

            _httpContext = new DefaultHttpContext();

            var controllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };

            superAdminController = new SuperAdminController(
                _superAdminService.Object,
                _resourceManager.Object)
            {
                ControllerContext = controllerContext,
            };
        }

        [Fact]
        public async Task AddDirection_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.AddDirection(It.IsAny<DirectionPostApiModel>())).ReturnsAsync(response);

            //Act
            var result = await superAdminController.AddDirection(It.IsAny<DirectionPostApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(null, "sms")]
        [InlineData("sms", null)]
        public async Task AddInstitutionOfEducationAdmin_EndpointsReturnBadRequest_IfModelStateIsNotValid(string adminEmail, string institutionOfEducationId)
        {
            // Arrange
            var inst = new InstitutionOfEducationAdminApiModel() { AdminEmail = adminEmail, InstitutionOfEducationId = institutionOfEducationId };

            // Act
            // Assert
            var exeption = await Assert.ThrowsAsync<NullReferenceException>(() => superAdminController.AddInstitutionOfEducationAdmin(inst));
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
        public async Task DisableInstitutionOfEducationAdmin_EndpointsReturnsResponseApiModel_or_Exception(bool success, string message)
        {
            // Arrange
            var requestModel = new SchoolUniAdminDeleteApiModel { Id = "id" };
            var responseModel = new ResponseApiModel<IoEAdminForSuperAdminResponseApiModel>(new IoEAdminForSuperAdminResponseApiModel(), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _superAdminService.Setup(x => x.DisableInstitutionOfEducationAdmin(requestModel.Id)).Returns(Task.FromResult(responseModel));
                // Act
                var result = await superAdminController.DisableInstitutionOfEducationAdmin(requestModel.Id);
                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<IoEAdminForSuperAdminResponseApiModel>(responseResult.Value);
            }
            else
            {
                _superAdminService.Setup(x => x.DisableInstitutionOfEducationAdmin(requestModel.Id)).Throws(error);
                // Assert
                await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.DisableInstitutionOfEducationAdmin(requestModel.Id));
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

        [Fact]
        public async Task GetAllUniAdminUsersAsync_EndpointReturnAllUsers()
        {
            // Arrange
            var responseApiModel = new PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>() 
                { ResponseList = InstitutionOfEducationAdminTestData.GetInstitutionOfEducationAdminResponseApiModels()};

            _superAdminService.Setup(x => x.GetAllInstitutionOfEducationAdmins(It.IsAny<InstitutionOfEducationAdminSortingModel>(), It.IsAny<PageApiModel>()))
                .Returns(Task.FromResult(responseApiModel));

            // Act
            var result = await superAdminController.GetAllInstitutionOfEducationsAdmins(null, null, null, null, 1, 10);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>)responseResult.Value;
            Assert.Equal(responseApiModel.ResponseList, model.ResponseList);
        }

        [Fact]
        public async Task GetAllUniAdminUsersAsync_EndpointReturnNothing()
        {
            // Arrange
            var responseApiModel = new PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>()
            { ResponseList = new List<InstitutionOfEducationAdminResponseApiModel>() };

            _superAdminService.Setup(x => x.GetAllInstitutionOfEducationAdmins(It.IsAny<InstitutionOfEducationAdminSortingModel>(), It.IsAny<PageApiModel>()))
                .Returns(Task.FromResult(responseApiModel));

            // Act
            var result = await superAdminController.GetAllInstitutionOfEducationsAdmins(null, null, null, null, 1, 10);

            // Assert
            var responseResult = Assert.IsType<OkObjectResult>(result);
            var model = (PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>)responseResult.Value;
            Assert.Empty(model.ResponseList);
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
                new SchoolAdminResponseApiModel {Id="Id",SchoolId="Id",SchoolName="InstitutionOfEducationId"}
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

        [Fact]
        public async Task UpdateSpecialtyById_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.UpdateSpecialtyById(It.IsAny<SpecialtyPutApiModel>())).ReturnsAsync(response);

            //Act
            var result = await superAdminController.UpdateSpecialtyById(It.IsAny<SpecialtyPutApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetModeratorsByIoEId_ShouldReturnOk_IfEverythingIsOk()
        {
            // Arrange  
            _superAdminService.Setup(x => x.GetIoEModeratorsByIoEId(It.IsAny<string>()))
                .ReturnsAsync(new ResponseApiModel<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>());

            // Act
            var result = await superAdminController.GetModeratorsByIoEId(It.IsAny<string>());

            // Assert  
            Assert.IsType<OkObjectResult>(result);
        }

        private List<AuthenticateResponseApiModel> GetTestJwt()
        {
            return new List<AuthenticateResponseApiModel>
            {
                new AuthenticateResponseApiModel { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmZWY1ZmM4LTA1NjEtNDI2OS04Zjc1LTk1N2RhNzg4ODkyOCIsImVtYWlsIjoicXRvbmk2QGdtYWlsLmNvbSIsIm5hbWUiOiJBcm5vbGRCZWFzbGV5Iiwicm9sZXMiOiJVbml2ZXJzaXR5QWRtaW4iLCJleHAiOjE2MDg1MDQxMjl9.araGavMMEaMXF2fjFU_OH72ipfJuae21vzxEcfTp_L0" },
                new AuthenticateResponseApiModel { Token = null },
            };
        }

        [Fact]
        public async Task AddSpecialtyToListOfSpecialties_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.AddSpecialtyToTheListOfAllSpecialties(It.IsAny<SpecialtyPostApiModel>())).ReturnsAsync(response);

            //Act
            var result = await superAdminController.AddSpecialtyToTheListOfAllSpecialties(It.IsAny<SpecialtyPostApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(true, "success")]
        [InlineData(false, "wrong")]
        public async Task DisableInstitutionOfEducation_EndpointsReturnsResponseApiModelWithText_or_Exception(bool success, string message)
        {
            // Arrange
            var requestId = "04b9a0c9-2545-4e28-9920-478aa6031c4b";
            var responseModel = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(message), true);
            var error = new NotFoundException(message);

            if (success)
            {
                _superAdminService.Setup(x => x.ChangeBannedStatusOfIoE(requestId)).Returns(Task.FromResult(responseModel));
                // Act
                var result = await superAdminController.BanInstitutionOfEducation(requestId);
                // Assert
                var responseResult = Assert.IsType<OkObjectResult>(result);
                var model = (DescriptionResponseApiModel)responseResult.Value;
                Assert.Equal(responseModel.Object.Message, model.Message);
            }
            else
            {
                _superAdminService.Setup(x => x.ChangeBannedStatusOfIoE(requestId)).Throws(error);
                // Assert
                var exсeption = await Assert.ThrowsAsync<NotFoundException>(() => superAdminController.BanInstitutionOfEducation(requestId));
                Assert.Equal(error.Message, exсeption.Message);
            }
        }

        [Fact]
        public async Task ChooseIoEAdminFromModerators_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService
                .Setup(x => x.ChooseIoEAdminFromModerators(It.IsAny<IoEAdminAddFromModeratorsApiModel>()))
                .ReturnsAsync(response);

            //Act
            var result = await superAdminController.ChooseIoEAdminFromModerators(It.IsAny<IoEAdminAddFromModeratorsApiModel>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteInstitutionOfEducation_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.DeleteInstitutionOfEducation(It.IsAny<string>()))
                .ReturnsAsync(response);

            //Act
            var result = await superAdminController.DeleteInstitutionOfEducation(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetIoEAdminIdByIoEId_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.GetIoEAdminIdByIoEId(It.IsAny<string>()))
                .ReturnsAsync(response);

            //Act
            var result = await superAdminController.GetIoEAdminIdByIoEId(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeletedSpecialty_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            
            _superAdminService.Setup(x => x.DeleteSpecialty(It.IsAny<string>())).ReturnsAsync(response);

            //Act
            var result = await superAdminController.DeleteSpecialty(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetIoEInfoByIoEId_EndpointReturnsOk()
        {
            //Arrange
            var response = new ResponseApiModel<IoEforSuperAdminResponseApiModel>(new IoEforSuperAdminResponseApiModel(), true);
            _superAdminService.Setup(x => x.GetIoEInfoByIoEId(It.IsAny<string>()))
                .ReturnsAsync(response);

            //Act
            var result = await superAdminController.GetIoEInfoByIoEId(It.IsAny<string>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ModifyInstitution_ShouldReturnBadRequest()
        {
            // Arrange
            JsonPatchDocument<InstitutionOfEducationPostApiModel> operations = null;

            //Act
            Func<Task> act = () => superAdminController.ModifyIoE(operations, It.IsAny<string>());

            //Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task ModifyInstitution_ShouldReturnOk()
        {
            //Arrange
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _superAdminService.Setup(x => x.ModifyIoE(It.IsAny<string>(), It.IsAny<JsonPatchDocument<InstitutionOfEducationPostApiModel>>()))
                .ReturnsAsync(response);

            // Act
            var result = await superAdminController.ModifyIoE(new JsonPatchDocument<InstitutionOfEducationPostApiModel>(), It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
