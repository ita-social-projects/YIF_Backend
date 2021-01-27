using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class UniversityControllerTests
    {
        private readonly HttpClient _client;

        public UniversityControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44324/api/University")
            };

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        #region CorrectTests

        [Theory]
        [InlineData("%D0%9C%D0%B0%D1%82%D0%B5%D0%BC%D0%B0%D1%82%D0%B8%D0%BA%D0%B0%20%D1%82%D0%B0%20%D1%81%D1%82%D0%B0%D1%82%D0%B8%D1%81%D1%82%D0%B8%D0%BA%D0%B0")]
        public async Task GET_EndpointsReturnUniversities_IfDirectionNameCorrect(string DirectionName)
        {
            // Act
            var response = await _client.GetAsync($"?DirectionName={DirectionName}");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JArray.Parse(JObject.Parse(content).GetValue("responseList").ToString());

            // Assert
            response.EnsureSuccessStatusCode();
            
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(contentJsonObj.Count > 1);
        }

        [Theory]
        [InlineData("%D0%9A%D0%BE%D0%BC%D0%BF%27%D1%8E%D1%82%D0%B5%D1%80%D0%BD%D1%96%20%D0%BD%D0%B0%D1%83%D0%BA%D0%B8")]
        public async Task GET_EndpointsReturnUniversities_IfSpecialityNameCorrect(string SpecialityName)
        {
            // Act
            var response = await _client.GetAsync($"?SpecialityName={SpecialityName}");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JArray.Parse(JObject.Parse(content).GetValue("responseList").ToString());

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(contentJsonObj.Count >= 1);
        }

        [Theory]
        [InlineData("%D0%9A%D0%B8%D1%97%D0%B2%D1%81%D1%8C%D0%BA%D0%B8%D0%B9%20%D0%BF%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85%D0%BD%D1%96%D1%87%D0%BD%D0%B8%D0%B9%20%D1%96%D0%BD%D1%81%D1%82%D0%B8%D1%82%D1%83%D1%82%20%D1%96%D0%BC%D0%B5%D0%BD%D1%96%20%D0%86%D0%B3%D0%BE%D1%80%D1%8F%20%D0%A1%D1%96%D0%BA%D0%BE%D1%80%D1%81%D1%8C%D0%BA%D0%BE%D0%B3%D0%BE")]
        public async Task GET_EndpointsReturnUniversities_IfUniversityNameCorrect(string UniversityName)
        {
            // Act
            var response = await _client.GetAsync($"?UniversityName={UniversityName}");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JArray.Parse(JObject.Parse(content).GetValue("responseList").ToString());

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(contentJsonObj.Count == 1);
        }
        
        [Theory]
        [InlineData("%D0%86%D0%BD%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D1%86%D1%96%D0%B9%D0%BD%D1%96%20%D1%82%D0%B5%D1%85%D0%BD%D0%BE%D0%BB%D0%BE%D0%B3%D1%96%D1%97", "%D0%A1%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%BD%D0%B8%D0%B9%20%D0%B0%D0%BD%D0%B0%D0%BB%D1%96%D0%B7")]
        public async Task GET_EndpointsReturnUniversities_IfDirectionName_And_SpecialityNameCorrect(string DirectionName, string SpecialityName)
        {
            // Act
            var response = await _client.GetAsync($"?DirectionName={DirectionName}&SpecialityName={SpecialityName}&page=1&pageSize=10");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JArray.Parse(JObject.Parse(content).GetValue("responseList").ToString());

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(contentJsonObj.Count >= 1);
        }

        #endregion

        #region InCorrectTests
        [Theory]
        [InlineData("PazhiloyDirection")]
        public async Task GET_EndpointsReturnUniversities_IfDirectionNameInCorrect(string DirectionName)
        {
            // Act
            var response = await _client.GetAsync($"?DirectionName={DirectionName}");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content).GetValue("message").ToString();

            // Assert            
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        #endregion
    }
}
