using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunctionTutorial.Functions
{
    public class JsonMessage
    {
        [property: JsonPropertyName("from")]
        public required string From { get; set; }
        [property: JsonPropertyName("message")]
        public required string Message { get; set; }
    }

        public class Function2
    {
        private readonly ILogger<Function2> _logger;
        private readonly QueueClient _queueClient;

        public Function2(ILogger<Function2> logger)
        {
            _logger = logger;

            // Retrieve the Service Bus connection string from the environment variable
            string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsQueueServiceConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("AzureWebJobsQueueServiceConnectionString is missing from the environment variables");
            }

            // Retrieve the queue name from the environment variable
            string? queueName = Environment.GetEnvironmentVariable("QueueName");
            if (string.IsNullOrEmpty(queueName))
            {
                throw new InvalidOperationException("QueueName is missing from the environment variables");
            }

            // Create a QueueClient instance using the connection string
            _queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });

        }

        [Function("Function2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            // Read the request body
            string requestBody;
            using (StreamReader reader = new StreamReader(req.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            // Log the raw request body for debugging
            _logger.LogInformation($"Request Body: {requestBody}");


            JsonMessage? json;
            try
            {
                json = JsonSerializer.Deserialize<JsonMessage>(requestBody);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize JSON");
                return new BadRequestObjectResult("Invalid JSON format");
            }

            string? from = json?.From;

            if(from == null)
            {
                return new BadRequestObjectResult("Please pass a name in the request body");
            }

            string? message = json?.Message;

            _logger.LogInformation($"From: {from}");
            _logger.LogInformation($"Message: {message}");

            if(message == null)
            {
                return new BadRequestObjectResult("Please pass a message in the request body");
            }


            Message record = new() { PartitionKey = "DEMO", From = from, MessageContent = message };

            // Create a Service Bus sender
            await _queueClient.CreateIfNotExistsAsync();
            await _queueClient.SendMessageAsync(JsonSerializer.Serialize(record));

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkResult();
        }
    }
}
