using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace FunctionTutorial
{
    public class MessageRecordService : IMessageRecordService
    {
        private readonly ILogger<MessageRecordService> _logger;
        private readonly TableClient _tableClient;

        public MessageRecordService (ILogger<MessageRecordService> logger)
        {
            _logger = logger;

            string? connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            string? table = Environment.GetEnvironmentVariable("TableName");

            _tableClient = new TableClient(connection, table);
        }

        public async Task ProcessMessageAsync(Message message)
        {
            if (message == null) throw new ArgumentNullException("Trying to write null message");

            await _tableClient.CreateIfNotExistsAsync();

            await _tableClient.AddEntityAsync(message);
        }

        public IEnumerable<Message> GetAll()
        {
            return _tableClient.Query<Message>(filter: "", maxPerPage: 100);
        }
        public async Task<Azure.AsyncPageable<Message>> GetNewMessages(DateTimeOffset since)
        {
            var messages = _tableClient.QueryAsync<Message>(
                filter: $"Timestamp ge datetime'{since.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}'",
                maxPerPage: 100);

            return messages;
        }

    }
}
