using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
       .ConfigureAppConfiguration(builder =>
       {
           builder.AddAzureAppConfiguration(options =>
           {
               options.Connect(Environment.GetEnvironmentVariable("AppConfiguration"))
                       // Load all keys that start with `TestApp:` and have no label
                       .Select("TestApp:*")
                       // Configure to reload configuration if the registered sentinel key is modified
                       .ConfigureRefresh(refreshOptions =>
                           refreshOptions.Register("refreshAll", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(5)));
           });

       })
       .ConfigureServices(services =>
       {
           // Make Azure App Configuration services available through dependency injection.
           services.AddAzureAppConfiguration();
       })
        .ConfigureFunctionsWorkerDefaults(app =>
        {
            // Use Azure App Configuration middleware for data refresh.
            app.UseAzureAppConfiguration();
        })
      
       .Build();

host.Run();

