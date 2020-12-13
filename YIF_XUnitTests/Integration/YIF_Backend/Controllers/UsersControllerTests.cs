using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class UsersControllerTests 
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string _root;
        public UsersControllerTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
            _root = "https://localhost:44324/api/Users";
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/00f5e3b3-fa90-4e74-856f-cbd41f178529")]
        public async Task GetUserTest(string url)
        {
            // Act            
            var response = await _client.GetAsync(_root + url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
