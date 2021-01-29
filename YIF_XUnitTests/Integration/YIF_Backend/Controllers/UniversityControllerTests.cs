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
        [InlineData("Автоматизація та приладобудування")]
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
        [InlineData("Статистика")]
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
        [InlineData("Академія внутрішніх військ МВС України")]
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
        [InlineData("Інформаційні технології", "Системний аналіз")]
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
