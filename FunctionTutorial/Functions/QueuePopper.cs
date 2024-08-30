using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionTutorial.Functions
{
    public class QueuePopper
    {
        private readonly ILogger<QueuePopper> _logger;
        private readonly IMessageRecordService _messageRecordService;

        public QueuePopper(ILogger<QueuePopper> logger, IMessageRecordService messageRecordService)
        {
            _logger = logger;
            _messageRecordService = messageRecordService;
        }

        [Function(nameof(QueuePopper))]
        public async Task Run([QueueTrigger("messages", Connection = "QueueServiceConnectionString")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            if (message.DequeueCount > 5)
            {
                _logger.LogError($"Message has been dequeued {message.DequeueCount} times. Message will be discarded.");
                return;
            }

            if (string.IsNullOrEmpty(message.MessageText))
            {
                throw new InvalidOperationException("Message is empty");
            }

            try
            {
                Message? record = JsonSerializer.Deserialize<Message>(message.MessageText);

                if (record != null)
                {
                    await _messageRecordService.ProcessMessageAsync(record);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing message");
            }
        }
    }
}
