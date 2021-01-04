using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IMyMessageSender
    {
        Task Send(string content);
    }

    public class AzureQueueSender : IMyMessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AzureQueueSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("QueueConnectionString");
        }

        public async Task Send(string content)
        {
            await SendMessage(content);
        }

        private async Task SendMessage(string content)
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            storageAccount.CreateCloudQueueClient();
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("yifconfig");
            var message = new CloudQueueMessage(content);
            await queue.AddMessageAsync(message);
        }
    }
}
