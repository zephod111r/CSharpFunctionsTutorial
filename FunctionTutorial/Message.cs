using Azure;
using Azure.Data.Tables;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunctionTutorial
{
    public record Message : ITableEntity
    {
        // ITableEntry definitions
        [property: JsonPropertyName("conversationId")]
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString();

        [property: JsonPropertyName("messageId")]
        public string RowKey { get; set; } = Guid.NewGuid().ToString();

        [property: JsonPropertyName("timestamp")]
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;

        [property: JsonPropertyName("tags")]
        public ETag ETag { get; set; }

        // Local properties
        [property: JsonPropertyName("from")]
        public required string From { get; init; }

        [property: JsonPropertyName("message")]
        public required string MessageContent { get; init; }

        // Constructor from JSON
        public Message()
        { }

        // Constructor from JSON
        public Message(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var message = JsonSerializer.Deserialize<Message>(json, options);

            if (message == null || string.IsNullOrEmpty(message.PartitionKey) || string.IsNullOrEmpty(message.RowKey)) throw new InvalidDataException("Invalid Json object passed");

            PartitionKey = message.PartitionKey;
            RowKey = message.RowKey;
            Timestamp = message.Timestamp;
            ETag = message.ETag;
            From = message.From;
            MessageContent = message.MessageContent;
        }

        // Copy constructor
        public Message(Message other)
        {
            PartitionKey = other.PartitionKey;
            RowKey = other.RowKey;
            Timestamp = other.Timestamp;
            ETag = other.ETag;
            From = other.From;
            MessageContent = other.MessageContent;
        }
    }
}
