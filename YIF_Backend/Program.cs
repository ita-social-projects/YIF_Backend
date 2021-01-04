using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace YIF_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog((hostingContext, loggerConfig) =>
                    loggerConfig.ReadFrom
                    .Configuration(hostingContext.Configuration));

        //.ConfigureAppConfiguration((context, config) =>
        //{
        //    var builtConfig = config.Build();
        //    var vaultName = builtConfig["VaultName"];
        //    var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
        //        {
        //            var credential = new DefaultAzureCredential(false);
        //            var token = await credential.GetTokenAsync(
        //                new Azure.Core.TokenRequestContext(
        //                    new[] { "https://vault.azure.net/.default" }));
        //            return token.Token;
        //        });

        //    config.AddAzureKeyVault(vaultName, keyVaultClient, new DefaultKeyVaultSecretManager());
        //});
    }
}
