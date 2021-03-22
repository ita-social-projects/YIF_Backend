using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class DirectionControllerTest : TestServerFixture
    {
        public DirectionControllerTest(ApiWebApplicationFactory fixture)
          : base(fixture) { }

        [Theory]
        [InlineData("/api/Direction/All")]
        [InlineData("/api/Direction/All?page=1")]
        [InlineData("/api/Direction/All?page=1&pageSize=10")]
        [InlineData("/api/Direction/All?DirectionName=Інформаційні технології")]
        [InlineData("/api/Direction/All?DirectionName=Інформаційні технології&SpecialtyName=Кібербезпека&InstitutionOfEducationId=Київський політехнічний інститут імені Ігоря Сікорського&InstitutionOfEducationAbbreviation=КПІ")]
        [InlineData("/api/Direction/All?DirectionName=Інформаційні технології&SpecialtyName=Кібербезпека&InstitutionOfEducationId=Київський політехнічний інститут імені Ігоря Сікорського&InstitutionOfEducationAbbreviation=КПІ&page=1&pageSize=10")]
        public async Task GetAll_EndpointsReturnSuccessAndCorrectContentObject(string endpoint)
        {
            // Act            
            var response = await _client.GetAsync(endpoint);

            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<IEnumerable<DirectionResponseApiModel>>(stringResponse);
            Assert.NotEmpty(models);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("Інформаційні технології", "", "", "")]
        [InlineData("Інформаційні технології", "Кібербезпека", "", "")]
        [InlineData("Інформаційні технології", "Кібербезпека", "Київський політехнічний інститут імені Ігоря Сікорського", "")]
        [InlineData("Інформаційні технології", "Кібербезпека", "Київський політехнічний інститут імені Ігоря Сікорського", "КПІ")]
        public async Task GetDirectionNames_EndpointsReturnSuccessAndCorrectContentObject(string directionName, string specialtyName,
            string institutionOfEducationName, string institutionOfEducationAbbreviation)
        {
            // Act            
            var response = await _client.GetAsync($"/api/Direction/Names?DirectionName={directionName}&SpecialtyName={specialtyName}&InstitutionOfEducationId={institutionOfEducationName}&InstitutionOfEducationAbbreviation={institutionOfEducationAbbreviation}");
            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<IEnumerable<string>>(stringResponse);
            Assert.NotEmpty(models);
        }
    }
}
