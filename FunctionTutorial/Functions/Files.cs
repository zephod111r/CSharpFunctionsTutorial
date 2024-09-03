using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionTutorial.Functions
{
    public class Files
    {
        private readonly ILogger<Files> _logger;

        public Files(ILogger<Files> logger)
        {
            _logger = logger;
        }

        [Function("Files")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Files/{path}")] HttpRequest req,
            string? path)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            if (string.IsNullOrEmpty(path))
            {
                path = req.Query["path"];
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            string extension = Path.GetExtension(path);

            string body = Utility.GetDynamicContent(path, extension);

            string contentType = Utility.GetContentType(extension);

            return new ContentResult
            {
                Content = body,
                ContentType = contentType,
                StatusCode = StatusCodes.Status200OK
            };
        }

    }
}
