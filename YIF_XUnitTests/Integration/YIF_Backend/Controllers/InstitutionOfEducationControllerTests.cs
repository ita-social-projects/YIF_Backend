using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF_Backend;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class InstitutionOfEducationControllerTests : TestServerFixture
    {
        private readonly InstitutionOfEducationInputAttribute _institutionOfEducationInputAttribute;
        public InstitutionOfEducationControllerTests(ApiWebApplicationFactory fixture) : base (fixture)
        {
            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _institutionOfEducationInputAttribute = new InstitutionOfEducationInputAttribute(_context);
            _institutionOfEducationInputAttribute.SetUserIdByGraduateUserIdForHttpContext();
        }

        #region CorrectTests

        [Theory]
        [InlineData("Автоматизація та приладобудування")]
        public async Task GET_EndpointsReturnInstitutionOfEducations_IfDirectionNameCorrect(string DirectionName)
        {
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Anonymous?DirectionName={DirectionName}");
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
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Anonymous?SpecialtyName={specialtyName}");
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
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Anonymous?InstitutionOfEducationName={institutionOfEducationName}");
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
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Anonymous?DirectionName={directionName}&SpecialtyName={specialtyName}&page=1&pageSize=10");
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
        [InlineData("Автоматизація та приладобудування")]
        public async Task GETAuthorized_EndpointsReturnInstitutionOfEducations_IfDirectionNameCorrect(string DirectionName)
        {
            
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Authorized?DirectionName={DirectionName}");
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
        public async Task GETAuthorized_EndpointsReturnInstitutionOfEducations_IfSpecialtyNameCorrect(string specialtyName)
        {
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Authorized?SpecialtyName={specialtyName}");
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
        public async Task GETAuthorized_EndpointsReturnInstitutionOfEducations_IfInstitutionOfEducationNameCorrect(string institutionOfEducationName)
        {
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Authorized?InstitutionOfEducationName={institutionOfEducationName}");
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
        public async Task GETAuthorized_EndpointsReturnInstitutionOfEducations_IfDirectionName_And_SpecialityNameCorrect(string directionName, string specialtyName)
        {
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Authorized?DirectionName={directionName}&SpecialtyName={specialtyName}&page=1&pageSize=10");
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
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Anonymous?DirectionName={directionName}");
            var content = response.Content.ReadAsStringAsync().Result;

            var contentJsonObj = JObject.Parse(content).GetValue("message").ToString();

            // Assert            
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("PazhiloyDirection")]
        public async Task GETAuthorized_EndpointsReturnInstitutionOfEducations_IfDirectionNameInCorrect(string directionName)
        {
            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducation/Authorized?DirectionName={directionName}");
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
