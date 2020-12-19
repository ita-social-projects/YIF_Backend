using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationControllerTests
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions();
            clientOptions.BaseAddress = new Uri("https://localhost:44324/api/Authentication/LoginUser/");

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        [Theory]
        [InlineData("")]
        public async Task Post_EndpointsReturnJwt_IfLoginAndPasswordCorrect(string url)
        {
            // Arrange
            var user = new User
            {
                Email = "qtoni6@gmail.com",
                Password = "QWerty-1"
            };

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(url, data);
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content);

            var successStatus = contentJsonObj["success"].ToObject<bool>();
            var token = new JwtSecurityToken(contentJsonObj["object"]["userToken"].ToString());

            var payload = token.Payload;

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(successStatus);
            Assert.Equal(user.Email, payload["email"]);
        }

        [Theory]
        [InlineData("")]
        public async Task Post_EndpointsReturnError_IfLoginIncorrect(string url)
        {
            // Arrange
            var user = new User
            {
                Email = "d@gmail.com",
                Password = "QWerty-1"
            };

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(url, data);
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content);

            var successStatus = contentJsonObj["success"].ToObject<bool>();
            var token = contentJsonObj["object"]["userToken"].ToObject<object>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.False(successStatus);
            Assert.Null(token);
        }

        [Theory]
        [InlineData("")]
        public async Task Post_EndpointsReturnError_IfPasswordIncorrect(string url)
        {
            // Arrange
            var user = new User
            {
                Email = "qtoni6@gmail.com",
                Password = "d"
            };

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(url, data);
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content);

            var successStatus = contentJsonObj["success"].ToObject<bool>();
            var token = contentJsonObj["object"]["userToken"].ToObject<object>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.False(successStatus);
            Assert.Null(token);
        }
    }
}
