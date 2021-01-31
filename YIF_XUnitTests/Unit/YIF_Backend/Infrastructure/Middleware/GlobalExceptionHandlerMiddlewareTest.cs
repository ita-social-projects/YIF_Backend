using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;
using YIF_Backend.Infrastructure.Middleware;

namespace YIF_XUnitTests.Unit.YIF_Backend.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddlewareTest
    {
        [Theory]
        [InlineData(HttpStatusCode.Processing)]
        [InlineData(HttpStatusCode.NotExtended)]
        [InlineData(HttpStatusCode.NotImplemented)]
        public async Task Invoke_ShouldReturnHttpResponseException_OnHttpResponseException(HttpStatusCode code)
        {
            // Arrange
            var middleware = new GlobalExceptionHandlerMiddleware((innerHttpContext) =>
            {
                throw new HttpResponseException(code);
            });
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            // Assert
            await Assert.ThrowsAsync<HttpResponseException>(() => middleware.Invoke(context));
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetExceptionFromDataGenerator), MemberType = typeof(TestDataGenerator))]
        public async Task Invoke_ShouldChangeContextResponse_BasedOnInputException(Exception error, int statusCode)
        {
            //Arrange
            var middleware = new GlobalExceptionHandlerMiddleware((innerHttpContext) =>
            {
                throw error;
            });
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);
            var responseResult = context.Response;

            //Assert
            Assert.Equal("application/json; charset=utf-8", responseResult.ContentType);
            Assert.Equal(statusCode, responseResult.StatusCode);
        }
    }

    internal class TestDataGenerator
    {
        private const string message = "message";
        public static IEnumerable<object[]> GetExceptionFromDataGenerator()
        {
            yield return new object[] { new InvalidOperationException(message), 409 };
            yield return new object[] { new BadImageFormatException(message), 400 };
            yield return new object[] { new FormatException(message), 400 };
            yield return new object[] { new ArgumentNullException(message), 400 };
            yield return new object[] { new KeyNotFoundException(message), 404 };
            yield return new object[] { new NotImplementedException(message), 500 };
        }
    }
}
