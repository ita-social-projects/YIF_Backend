using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Xunit;
using YIF.Core.Data;

namespace YIF_XUnitTests.Integration.Fixture
{
    public abstract class TestServerFixture : IClassFixture<ApiWebApplicationFactory>
    {
        protected static HttpClient _client;
        private static object syncRoot = new Object();

        protected static ApiWebApplicationFactory _factory;
        protected static IConfiguration _configuration;
        protected static EFDbContext _context;

        public TestServerFixture()
        {
            
        }

        public static HttpClient getInstance(ApiWebApplicationFactory fixture)
        {
            if (_context == null)
            {
                lock (syncRoot)
                {
                    if (_context == null)
                    {

                        _factory = fixture;
                        _client = _factory.CreateClient();

                        _context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<EFDbContext>();
                    }
                }
            }
            return _client;
        }
    }
}
