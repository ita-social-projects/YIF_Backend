using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class LectorControllerTest : TestServerFixture
    {
        public LectorControllerTest(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_EndpointsReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/Lector/GetAllDepartments");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GettAllDisciplinesAsync_EndpointsReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/Lector/GetAllDisciplines");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
