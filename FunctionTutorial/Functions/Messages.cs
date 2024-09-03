using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FunctionTutorial.Functions
{
    public class Messages
    {
        private readonly ILogger<Messages> _logger;
        private readonly IMessageRecordService _records;

        public Messages(ILogger<Messages> logger, IMessageRecordService records)
        {
            _logger = logger;
            _records = records;
        }

        [Function("Messages")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                // Get the last request timestamp from the request headers
                string? lastRequestTimestamp = req.Headers["Last-Request-Timestamp"];

                if (string.IsNullOrEmpty(lastRequestTimestamp))
                {
                    return new BadRequestObjectResult("Last-Request-Timestamp header is missing or empty.");
                }

                _logger.LogInformation("Requested timestamp: {0}", lastRequestTimestamp);

                if (!DateTimeOffset.TryParse(lastRequestTimestamp, out DateTimeOffset timeOffset))
                {
                    return new BadRequestObjectResult("Invalid Last-Request-Timestamp format.");
                }

                // Retrieve new messages since the last request
                var newMessages = await _records.GetNewMessages(timeOffset);

                // Return the new messages as JSON
                Message[] messages = new Message[0];
                await foreach (Page<Message> page in newMessages.AsPages())
                {
                    messages = [.. messages, .. page.Values.Select(e => e).ToArray()];
                }

                var sortedmessages = messages.OrderBy(e => e.Timestamp);

                return new JsonResult(sortedmessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
