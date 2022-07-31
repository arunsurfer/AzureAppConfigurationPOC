using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace AzureAppConfigApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppConfigurationController : ControllerBase    {

        private readonly ILogger<AppConfigurationController> _logger;

        private readonly AppConfiguration _configuration;
        public AppConfigurationController(IOptionsSnapshot<AppConfiguration> configuration,ILogger<AppConfigurationController> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        [HttpGet(Name = "GetAppConfiguration")]
        public string Get()
        {
            return System.Text.Json.JsonSerializer.Serialize(_configuration);
        }
    }
}