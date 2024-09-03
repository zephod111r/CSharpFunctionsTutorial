using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionTutorial.Functions
{
    public class Index
    {
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger)
        {
            _logger = logger;
        }

        [Function("Index")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "index.html")] HttpRequest req,
            string? path)
        {
            string body = Utility.GetDynamicContent("index.html", ".html");

            string contentType = Utility.GetContentType(".html");

            return new ContentResult
            {
                Content = body,
                ContentType = contentType,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
