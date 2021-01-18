using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
        [InlineData(201, "UniName","email@gmailcom", "Password1+")]
        [InlineData(201, "", "email@gmailcom", "Password1+")]
        public async Task AddUniAdmin_EndpointsReturnResponseApiModelWithJwt_IfDataСorrect(int statusCode, string uniName, string email, string password)
        {
            // Arrange
            var requestModel = new UniversityAdminApiModel
            {
                UniversityName = uniName,
                Email = email,
                Password = password
                
            };

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { StatusCode = statusCode, Object = GetTestJwt()[0] };
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
        public async Task AddUniAdmin_EndpointsReturnErrorNoUniversityWithSuchName_IfDataСorrect(string uniName, string email, string password)
        {
            // Arrange
            var requestModel = new UniversityAdminApiModel
            {
                UniversityName = uniName,
                Email = email,
                Password = password

            };

            var ErrorMessage = "ExampleErrorMessage";
            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { StatusCode = 409, Message = ErrorMessage };
            _superAdminService.Setup(x => x.AddUniversityAdmin(requestModel)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await superAdminController.AddUniversityAdmin(requestModel);
            // Assert
            var responseResult = Assert.IsType<ConflictObjectResult>(result);
            var model = (DescriptionResponseApiModel)responseResult.Value;
            Assert.Equal(ErrorMessage, model.Message);
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

            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { StatusCode = 201, Object = GetTestJwt()[0] };
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
        public async Task AddSchoolAdmin_EndpointsReturnErrorNoUniversityWithSuchName_IfDataСorrect(string schoolName, string email, string password)
        {
            // Arrange
            var requestModel = new SchoolAdminApiModel
            {
                SchoolName = schoolName,
                Email = email,
                Password = password

            };

            var ErrorMessage = "ExampleErrorMessage";
            var responseModel = new ResponseApiModel<AuthenticateResponseApiModel> { StatusCode = 409, Message = ErrorMessage };
            _superAdminService.Setup(x => x.AddSchoolAdmin(requestModel)).Returns(Task.FromResult(responseModel));

            // Act
            var result = await superAdminController.AddSchoolAdmin(requestModel);
            // Assert
            var responseResult = Assert.IsType<ConflictObjectResult>(result);
            var model = (DescriptionResponseApiModel)responseResult.Value;
            Assert.Equal(ErrorMessage, model.Message);
        }


        //[Theory]
        //[InlineData("okId")]
        //public async Task DeleteSchoolAdmin_ReturnsResponseMessage_IfDataСorrect(string schoolAdminId)
        //{
        //    // Arrange
        //    var requestModel = new SchoolUniAdminDeleteApiModel
        //    {
        //        Id = schoolAdminId

        //    };

        //    var responseModel = new ResponseApiModel<DescriptionResponseApiModel> { StatusCode = 201,Message = "User IsDeleted was updated" };
        //    _superAdminService.Setup(x => x.DeleteSchoolAdmin(requestModel)).Returns(Task.FromResult(responseModel));

        //    // Act
        //    var result = await superAdminController.DeleteSchoolAdmin(requestModel);
        //    // Assert
        //    var responseResult = Assert.IsType<CreatedResult>(result);
        //    var model = (DescriptionResponseApiModel)responseResult.Value;
        //    Assert.Equal("User IsDeleted was updated", model.Object.Message);
        //}



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
