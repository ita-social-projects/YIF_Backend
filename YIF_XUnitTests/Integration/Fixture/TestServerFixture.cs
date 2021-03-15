using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;
using YIF.Core.Data;

namespace YIF_XUnitTests.Integration.Fixture
{
    public abstract class TestServerFixture : IClassFixture<ApiWebApplicationFactory>
    {
        protected ApiWebApplicationFactory _factory;
        protected HttpClient _client;
        protected IConfiguration _configuration;
        protected EFDbContext _context;

        public TestServerFixture(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();

            _context = fixture.Services.CreateScope().ServiceProvider.GetRequiredService<EFDbContext>();
        }
    }
}
