using System;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using FunctionTutorial.Functions;
using Microsoft.QualityTools.Testing.Fakes;

namespace FunctionTutorial.Tests.Functions
{
    /*
    [TestClass]
    public class QueuePopperTests
    {
        private ILogger<QueuePopper> _logger;
        private IMessageRecordService _messageRecordService;
        private QueuePopper _queuePopper;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new StubILogger<QueuePopper>();
            _messageRecordService = new StubIMessageRecordService();
            _queuePopper = new QueuePopper(_logger, _messageRecordService);
        }

        [TestMethod]
        public async Task Run_WithValidMessage_ProcessesMessage()
        {
            /*
            var message = new QueueMessage
            {
                MessageText = "",
                DequeueCount = 2
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _queuePopper.Run(message));
            * /
            // Arrange
            await Task.CompletedTask;
        }
    }*/
}
