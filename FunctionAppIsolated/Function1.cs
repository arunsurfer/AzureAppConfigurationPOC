using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace FunctionAppIsolated
{
    public class Function1
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _configuration = configuration;
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

           

            var configuration = new AppConfiguration
            {
                Count = Convert.ToInt32(_configuration["TestApp:Settings:count"]),
                url = _configuration["TestApp:Settings:url"],
                FontSize = Convert.ToInt32(_configuration["TestApp:Settings:FontSize"]),
                FontColor = _configuration["TestApp:Settings:FontColor"],
                Message = _configuration["TestApp:Settings:Message"],
                token = _configuration["TestApp:Settings:token"]
            };


            string data = System.Text.Json.JsonSerializer.Serialize(configuration);

            response.WriteString(data);

            return response;
        }
    }
}
