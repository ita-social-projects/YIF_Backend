using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        private readonly LectorController lectorController;
        private readonly Mock<HttpContext> _httpContext;

        public LectorControllerTests() 
        {
            _lectorService = new Mock<ILectorService>();
            _httpContext = new Mock<HttpContext>();

            lectorController = new LectorController(
                _lectorService.Object);

            lectorController.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContext.Object
            };
        }

        [Fact]
        public async Task ModifyLector_ShouldReturnBadRequest()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            JsonPatchDocument<LectorApiModel> operations = null;

            //Act
            Func<Task> act = () => lectorController.ModifyLector(operations);

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task ModifyLector_ShouldReturnOk()
        {
            //Arrange
            var claims = new List<Claim>()
            {
                new Claim("id", "id"),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContext.SetupGet(hc => hc.User).Returns(claimsPrincipal);
            var response = new ResponseApiModel<DescriptionResponseApiModel>(new DescriptionResponseApiModel(), true);
            _lectorService.Setup(x => x.ModifyLector(It.IsAny<string>(), It.IsAny<JsonPatchDocument<LectorApiModel>>()))
                .ReturnsAsync(response);

            // Act
            var result = await lectorController.ModifyLector(new JsonPatchDocument<LectorApiModel>());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
