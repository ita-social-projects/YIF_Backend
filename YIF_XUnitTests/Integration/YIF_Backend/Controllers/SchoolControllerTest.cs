using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF_XUnitTests.Integration.Fixture;
using System.Linq;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SchoolControllerTest : TestServerFixture
    {
        public SchoolControllerTest(ApiWebApplicationFactory fixture)
          : base(fixture) {  }


        [Fact]
        public async Task GetAllSchoolsAsync()
        {
            // Act
            var response = await _client.GetAsync("/api/School");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAllSchoolNamesAsStringsAsync()
        {

            // Act
            var response = await _client.GetAsync("/api/School/GetAllSchoolNamesAsStringsAsync");

            // Assert
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<IEnumerable<string>>(stringResponse);
            Assert.NotEmpty(models);
        }

        [Fact]
        public async Task GetAllSchoolNamesAsStringsAsync_withEmptyDB()
        {
            // Arrange
            var schools = _context.Schools.ToList();
            _context.Schools.RemoveRange(_context.Schools);
            _context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/School/GetAllSchoolNamesAsStringsAsync");

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);

            _context.Schools.AddRange(schools);
            _context.SaveChanges();
        }
    }
}
