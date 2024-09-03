using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FunctionTutorial.Functions
{
    public class Delete
    {
        private readonly ILogger<Delete> _logger;
        private readonly IMessageRecordService _messageRecordService;


        public Delete(ILogger<Delete> logger, IMessageRecordService messageRecordService)
        {
            _logger = logger;
            _messageRecordService = messageRecordService;
        }

        [Function("Delete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                await _messageRecordService.DeleteAllMessagesAsync();
            }
            catch (Exception) { 
                return new BadRequestResult();
            }
            return new OkResult();
        }
    }
}
