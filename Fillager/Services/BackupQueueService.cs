using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Fillager.Services
{
    public class BackupQueueService : IDisposable, IBackupQueueService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public BackupQueueService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory() { HostName = configuration.GetValue<string>("RABBITMQ_BACKUP_QUEUE")};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = configuration.GetValue<string>("RABBITMQ_BACKUP_QUEUE_NAME");

            _channel.QueueDeclare(queue: _queueName,//queue name
                    durable: true,//keep msgs on disk in case rabbitmq crashes
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            _channel.BasicQos(0, 1, false);
            //0 = unlimited max size per msg.
            //1 = 1 msg may be delivered without ack. 
            //false = only apply to this channel.
        }

        private void PublishEvent(string msg)
        {            
            var body = Encoding.UTF8.GetBytes(msg);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; //make sure events don't get lost if rabbitmq restarts


            _channel.BasicPublish(exchange: "",
                routingKey: _queueName,
                basicProperties: properties,
                body: body);
        }

        public void SendBackupRequest(string bucketName, string fileGuid)
        {
            var msgObj = new FileRequestObject()
            {
                version = 1,
                bucketname = bucketName,
                fileguid = fileGuid
            };
            var msgString = JsonConvert.SerializeObject(msgObj);
            PublishEvent(msgString);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        private class FileRequestObject
        {
            public int version { get; set; }
            public string bucketname { get; set; }
            public string fileguid { get; set; }
        }
    }
}
