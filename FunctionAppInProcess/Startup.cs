using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(FunctionAppInProcess.Startup))]
namespace FunctionAppInProcess
{
    class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var secretConfiguration = new ConfigurationBuilder().AddUserSecrets<Startup>().Build();

            var connectionString = secretConfiguration["AppConfig:ServiceApiKey"];

            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                        // Load all keys that start with `TestApp:` and have no label
                        .Select("TestApp:*")
                        // Configure to reload configuration if the registered sentinel key is modified
                        .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("refreshAll", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(5)));
                        
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureAppConfiguration();
        }
    }
}
