﻿using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44324/api/Authentication/LoginUser/")
            };

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        [Theory]
        [InlineData("qtoni6@gmail.com", "QWerty-1")]
        public async Task Post_EndpointsReturnJwt_IfLoginAndPasswordCorrect(string email, string password)
        {
            // Arrange
            var user = new LoginApiModel
            {
                Email = email,
                Password = password
            };

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", data);
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content);

            var token = new JwtSecurityToken(contentJsonObj["token"].ToString());

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Equal(user.Email, token.Payload["email"]);
        }

        [Theory]
        [InlineData("d@gmail.com", "QWerty-1")]
        [InlineData("qtoni6@gmail.com", "d")]
        public async Task Post_EndpointsReturnError_IfLoginOrPasswordIncorrect(string email, string password)
        {
            // Arrange
            var user = new LoginApiModel
            {
                Email = email,
                Password = password
            };

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("", data);
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content);

            var message = contentJsonObj["message"].ToString();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
               response.Content.Headers.ContentType.ToString());
            Assert.NotEmpty(message);
        }
    }
}
