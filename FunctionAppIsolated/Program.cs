using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var secretConfiguration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var connectionString = secretConfiguration["AppConfig:ServiceApiKey"];

var host = new HostBuilder()
       .ConfigureAppConfiguration(builder =>
       {
           builder.AddAzureAppConfiguration(options =>
           {
               options.Connect(connectionString)
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

