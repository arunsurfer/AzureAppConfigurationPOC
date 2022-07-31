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

            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("AppConfiguration"))
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
