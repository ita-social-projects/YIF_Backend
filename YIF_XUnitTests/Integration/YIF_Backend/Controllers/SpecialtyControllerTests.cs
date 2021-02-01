using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SpecialtyControllerTests
    {
        private readonly HttpClient _client;

        public SpecialtyControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44324/api/Specialty/")
            };

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        [Fact]
        public async Task GetAll_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act            
            var response = await _client.GetAsync("All");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetNames_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act            
            var response = await _client.GetAsync("Names");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }


        [Theory]
        [InlineData("00f5e3b3-fa90-4e74-856f-000000000000")]
        public async Task GetById_EndpointReturnNotFound(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
