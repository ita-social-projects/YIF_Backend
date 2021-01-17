using Microsoft.AspNetCore.Mvc.Testing;
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
        [InlineData("6necum.how@silentsuite.com", "QWerty-1", "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async Task Post_EndpointsReturnJwt_IfLoginAndPasswordCorrect(string email, string password, string recaptcha)
        {
            // Arrange
            var user = new LoginApiModel
            {
                Email = email,
                Password = password,
                RecaptchaToken = recaptcha
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
        [InlineData("d@gmail.com", "QWerty-1", "recaptcha")]
        [InlineData("qtoni6@gmail.com", "d", "recaptcha")]
        public async Task Post_EndpointsReturnError_IfLoginOrPasswordIncorrect(string email, string password, string recaptcha)
        {
            // Arrange
            var user = new LoginApiModel
            {
                Email = email,
                Password = password,
                RecaptchaToken = recaptcha
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
