using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SuperAdminControllerTest : TestServerFixture
    {
        public SuperAdminControllerTest(ApiWebApplicationFactory fixture)
          : base(fixture)
        {
            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [Theory]
        [MemberData(nameof(SuperAdminInputAttribute.GetWrongData), MemberType = typeof(SuperAdminInputAttribute))]
        public async Task AddUniversityAndAdmin_Input_WrongUniversityPostApiModel_site(StringContent content)
        {
            // Act
            var response = await _client.PostAsync("/api/SuperAdmin/AddUniversityAndAdmin", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddUniversityAndAdmin_Output_WithCorectData()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddUniversityAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddUniversityAndAdmin_Output_ByAddingSameUniversityTwoTimes()
        {            
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddUniversityAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.UniversityAdminEmail = "name@gmail.com";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task AddUniversityAndAdmin_Output_ByAddingSameAdminTwoTimes()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddUniversityAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.Name = "name";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task GetAllUniversities()
        {
            // Arrange
            var request = "/api/SuperAdmin/GetAllUniversities";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DeleteUniversityAdmin()
        {
            // Arrange
            var admin = _context.UniversityAdmins.First();
            
            // Act
            var response = await _client.DeleteAsync(string.Format("/api/SuperAdmin/DeleteUniversityAdmin/{0}", admin.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DisableUniversityAdmin()
        {
            // Arrange
            var admin = _context.UniversityAdmins.First();

            // Act
            var response = await _client.PostAsync(string.Format("/api/SuperAdmin/DisableUniversityAdmin/{0}", admin.Id), ContentHelper.GetStringContent(admin));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}