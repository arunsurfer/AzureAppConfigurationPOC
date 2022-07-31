using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration;

namespace FunctionAppInProcess
{
    public class Function1
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationRefresher _configurationRefresher;

        public Function1(IConfiguration configuration, IConfigurationRefresherProvider refresherProvider)
        {
            _configuration = configuration;
            _configurationRefresher = refresherProvider.Refreshers.First();
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _configurationRefresher.TryRefreshAsync();

 

            var configuration = new AppConfiguration
            {
                Count = Convert.ToInt32(_configuration["TestApp:Settings:count"]),
                url = _configuration["TestApp:Settings:url"],
                FontSize = Convert.ToInt32(_configuration["TestApp:Settings:FontSize"]),
                FontColor = _configuration["TestApp:Settings:FontColor"],
                Message = _configuration["TestApp:Settings:Message"],
                token = _configuration["TestApp:Settings:token"]
        };

      
            dynamic data = System.Text.Json.JsonSerializer.Serialize(configuration);

            return data != null
                   ? (ActionResult)new OkObjectResult(data)
                   : new BadRequestObjectResult($"Please create a key-value with the key  in App Configuration.");

        }
    }
}
