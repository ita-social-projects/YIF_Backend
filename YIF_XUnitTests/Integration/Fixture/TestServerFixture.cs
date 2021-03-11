using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Xunit;

namespace YIF_XUnitTests.Integration.Fixture
{
    public abstract class TestServerFixture : IClassFixture<ApiWebApplicationFactory>
    {
        protected ApiWebApplicationFactory _factory;
        protected HttpClient _client;
        protected IConfiguration _configuration;

        public TestServerFixture(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }
    }
}
