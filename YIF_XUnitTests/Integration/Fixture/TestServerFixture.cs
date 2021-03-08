using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Xunit;

namespace YIF_XUnitTests.Integration.Fixture
{
    public abstract class TestServerFixture : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly IConfiguration _configuration;

        public TestServerFixture(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }
    }
}
