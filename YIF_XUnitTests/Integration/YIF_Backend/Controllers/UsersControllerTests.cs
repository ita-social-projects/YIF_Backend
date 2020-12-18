﻿using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
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
        [InlineData("5ae7d968-a06f-41e1-a8db-adf0428a9bf5")]
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
            Assert.Equal("text/plain; charset=utf-8",
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
            Assert.Equal("text/plain; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
