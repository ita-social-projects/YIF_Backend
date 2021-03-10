using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.Fixture
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.Testing.json")
                  .Build();

                config.AddConfiguration(integrationConfig);
            });

            builder.UseEnvironment("Testing");
        }
    }
}
