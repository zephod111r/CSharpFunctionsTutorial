using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTutorial
{
    public interface IMessageRecordService
    {
        public Task ProcessMessageAsync(Message message);
        public IEnumerable<Message> GetAll();

        public Task<Azure.AsyncPageable<Message>> GetNewMessages(DateTimeOffset since);


        public Task DeleteMessageAsync(Message message);

        public Task DeleteAllMessagesAsync();
    }
}
