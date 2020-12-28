using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class ResponseApiModelTests
    {
        [Theory]
        [InlineData(200, "message")]
        [InlineData(400)]
        public void Ctor_ShouldImplementParameters(int statusCode, string message = null)
        {
            // Act
            var testObject = new ResponseApiModel<double>(statusCode, message);
            //Assert
            Assert.Equal(statusCode, testObject.StatusCode);
            Assert.Equal(message, testObject.Message);
            Assert.Equal(typeof(double), testObject.Object.GetType());
        }

        [Theory]
        [InlineData(true, "message")]
        [InlineData(false)]
        public void Set_ShouldSetSuccessAndMessage_ReturnResponseApiModel(bool isSuccess, string message = null)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Set(isSuccess, message);
            //Assert
            Assert.IsType<ResponseApiModel<object>>(result);
            Assert.Equal(isSuccess, result.Success);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData(200, "message")]
        [InlineData(400)]
        public void Set_ShouldSetStatusAndMessage_ReturnResponseApiModel(int statusCode, string message = null)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Set(statusCode, message);
            //Assert
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData(200, 2.2, "message")]
        [InlineData(200, 7)]
        [InlineData(400, "text")]
        public void Set_ShouldSetStatusAndObjectAndMessage_ReturnResponseApiModel(int statusCode, object obj, string message = null)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Set(statusCode, obj, message);
            //Assert
            Assert.Equal(obj.GetType(), result.Object.GetType());
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(obj, result.Object);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(400)]
        public void Response_ShouldSetStatusCode_ReturnIActionResult(int statusCode)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Response(statusCode);
            //Assert
            Assert.Equal(statusCode, testObject.StatusCode);
            Assert.IsAssignableFrom<IActionResult>(result);
            if (statusCode == 200) Assert.IsType<OkResult>(result);
            if (statusCode == 400) Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Conflict)]
        public void Response_ShouldSetHttpStatusCode_ReturnIActionResult(HttpStatusCode status)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Response(status);
            //Assert
            Assert.Equal(status, (HttpStatusCode)testObject.StatusCode);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        [InlineData(202)]
        [InlineData(204)]
        [InlineData(400)]
        [InlineData(401)]
        [InlineData(403)]
        [InlineData(404)]
        [InlineData(409)]
        public void Response_ShouldSetStatusCode_ReturnIActionResultThatMeetsTheType(int statusCode)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>(statusCode);
            // Act
            var result = testObject.Response();
            //Assert
            Assert.Equal(statusCode, testObject.StatusCode);
            switch (statusCode)
            {
                case 200:
                    Assert.IsAssignableFrom<OkResult>(result);
                    break;
                case 201:
                    Assert.IsAssignableFrom<CreatedResult>(result);
                    break;
                case 202:
                    Assert.IsAssignableFrom<AcceptedResult>(result);
                    break;
                case 204:
                    Assert.IsAssignableFrom<NoContentResult>(result);
                    break;
                case 400:
                    Assert.IsAssignableFrom<BadRequestResult>(result);
                    break;
                case 401:
                    Assert.IsAssignableFrom<UnauthorizedResult>(result);
                    break;
                case 403:
                    Assert.IsAssignableFrom<ForbidResult>(result);
                    break;
                case 404:
                    Assert.IsAssignableFrom<NotFoundResult>(result);
                    break;
                case 409:
                    Assert.IsAssignableFrom<ConflictResult>(result);
                    break;
            }
        }
        [Theory]
        [InlineData(200, 7)]
        [InlineData(201, 7)]
        [InlineData(400, "error")]
        [InlineData(401, "error")]
        [InlineData(404, "error")]
        [InlineData(409, "error")]
        public void Response_ShouldSetStatusCode_ReturnIActionResultThatMeetsTheTypeWithObject(int statusCode, object obj)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>(statusCode, obj.ToString());
            testObject.Object = 5;
            // Act
            var result = testObject.Response();
            //Assert
            Assert.Equal(statusCode, testObject.StatusCode);
            switch (statusCode)
            {
                case 200:
                    var responseResult200 = Assert.IsAssignableFrom<OkObjectResult>(result);
                    var data200 = (int)responseResult200.Value;
                    Assert.Equal(testObject.Object, data200);
                    break;
                case 201:
                    var responseResult201 = Assert.IsAssignableFrom<CreatedResult>(result);
                    var data201 = (int)responseResult201.Value;
                    Assert.Equal(testObject.Object, data201);
                    break;
                case 400:
                    var responseResult400 = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
                    var model400 = (DescriptionResponseApiModel)responseResult400.Value;
                    Assert.Equal(testObject.Message, model400.Message);
                    break;
                case 401:
                    var responseResult401 = Assert.IsAssignableFrom<UnauthorizedObjectResult>(result);
                    var model401 = (DescriptionResponseApiModel)responseResult401.Value;
                    Assert.Equal(testObject.Message, model401.Message);
                    break;
                case 404:
                    var responseResult404 = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
                    var model404 = (DescriptionResponseApiModel)responseResult404.Value;
                    Assert.Equal(testObject.Message, model404.Message);
                    break;
                case 409:
                    var responseResult409 = Assert.IsAssignableFrom<ConflictObjectResult>(result);
                    var model409 = (DescriptionResponseApiModel)responseResult409.Value;
                    Assert.Equal(testObject.Message, model409.Message);
                    break;
            }
        }

        [Theory]
        [InlineData(110)]
        [InlineData(220)]
        [InlineData(330)]
        [InlineData(440)]
        [InlineData(550)]
        public void Response_ShouldSetStatusCodeNotImplement_WhenStatusCodeIsNotInSpecifiedList(int statusCode)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>(statusCode);
            // Act
            var result = testObject.Response();
            //Assert
            Assert.Equal((int)HttpStatusCode.NotImplemented, testObject.StatusCode);
        }
    }
}
