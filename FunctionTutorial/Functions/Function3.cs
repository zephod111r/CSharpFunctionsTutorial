using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionTutorial.Functions
{
    public class Function3
    {
        private readonly ILogger<Function3> _logger;

        public Function3(ILogger<Function3> logger)
        {
            _logger = logger;
        }

        [Function("Function3")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            string body = @"
                    body {
                        font-family: Arial, sans-serif;
                        font-size: 14px;
                        color: #333333;
                    }
                    .inputbox { 
                        width: 200px;
                        height: 30px;
                        border: 1px solid #cccccc;
                        border-radius: 4px;
                        padding: 5px;
                    }
                    .button {
                        background-color: #007bff;
                        color: #ffffff;
                        border: none;
                        border-radius: 4px;
                        padding: 10px 20px;
                        cursor: pointer;
                    }";

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new ContentResult
            {
                Content = body,
                ContentType = "text/css",
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
