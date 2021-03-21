using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class InstitutionOfEducationControllerTests
    {
        private readonly HttpClient _client;

        public InstitutionOfEducationControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44324/api/InstitutionOfEducation")
            };

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        #region CorrectTests

        [Theory]
        [InlineData("Автоматизація та приладобудування")]
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfDirectionNameCorrect(string DirectionName)
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
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfSpecialtyNameCorrect(string specialtyName)
        {
            // Act
            var response = await _client.GetAsync($"?SpecialtyName={specialtyName}");
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
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfInstitutionOfEducationNameCorrect(string institutionOfEducationName)
        {
            // Act
            var response = await _client.GetAsync($"?InstitutionOfEducationId={institutionOfEducationName}");
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
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfDirectionName_And_SpecialityNameCorrect(string directionName, string specialtyName)
        {
            // Act
            var response = await _client.GetAsync($"?DirectionName={directionName}&SpecialtyName={specialtyName}&page=1&pageSize=10");
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
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfDirectionNameInCorrect(string directionName)
        {
            // Act
            var response = await _client.GetAsync($"?DirectionName={directionName}");
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
