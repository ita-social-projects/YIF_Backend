using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class DepartmentControllerTests : TestServerFixture
    {
        public DepartmentControllerTests(ApiWebApplicationFactory fixture)
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
        public async Task AddDepartment_ShouldReturnOk()
        {
            //Arrange
            var postRequest = new
            {
                Url = "/api/Department/AddDepartment",
                Body = new DepartmentApiModel {Name = "FakeName", Description = "FakeDescription"}
            };

            //Act
            var responce = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //Assert
            responce.EnsureSuccessStatusCode();
        }
    }
}
