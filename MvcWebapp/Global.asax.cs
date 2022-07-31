using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using MVCWebAppNew.Models;

namespace MVCWebAppNew
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IConfiguration Configuration;
        private static IConfigurationRefresher _configurationRefresher;

        protected void Application_Start()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.AddAzureAppConfiguration(options =>
            {
                var connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

                options.Connect(connectionString)
                        // Load all keys that start with `TestApp:` and have no label.
                        .Select("TestApp:*")
                        // Configure to reload configuration if the registered key 'TestApp:Settings:Sentinel' is modified.
                        .ConfigureRefresh(refresh =>
                        {
                            refresh.Register("refreshAll", refreshAll: true)
                                   .SetCacheExpiration(TimeSpan.FromSeconds(5));
                        });
                _configurationRefresher = options.GetRefresher();
            });

            Configuration = builder.Build();
           // setConfiguration();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void setConfiguration()
        {
            AppConfiguration.Count = Convert.ToInt32(Configuration["TestApp:Settings:count"]);
            AppConfiguration.url = Configuration["TestApp:Settings:url"];
            AppConfiguration.FontSize = Convert.ToInt32(Configuration["TestApp:Settings:FontSize"]);
            AppConfiguration.FontColor = Configuration["TestApp:Settings:FontColor"];
            AppConfiguration.Message = Configuration["TestApp:Settings:Message"];
            AppConfiguration.token = Configuration["TestApp:Settings:token"];
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _ = _configurationRefresher.TryRefreshAsync();
            setConfiguration();
        }
    }
}
