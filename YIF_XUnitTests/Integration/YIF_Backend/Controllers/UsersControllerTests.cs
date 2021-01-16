using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class UsersControllerTests 
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public UsersControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions();
            clientOptions.BaseAddress = new Uri("https://localhost:44324/api/Users/");

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        [Theory]
        [InlineData("")]
        [InlineData("6f08c1b6-468d-4c44-afeb-488ded1ccb98")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("00f5e3b3-fa90-4e74-856f-cbd41f178520")]
        public async Task Get_EndpointReturnNotFound(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("d")]
        public async Task Get_EndpointReturnBadRequest(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("intelIdea@gmail.com")]
        public async Task Send_ResetPassword_IfEmail_Correct(string email)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ResetPasswordByEmailApiModel
            {
                UserEmail = email
            }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("ResetPassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("notExist@gmail.com")]
        public async Task Send_ResetPassword_IfEmail_InCorrect(string email)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ResetPasswordByEmailApiModel
            {
                UserEmail = email
            }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("ResetPassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
