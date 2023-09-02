using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Producer.Api.Interfaces;
using Producer.Api.Models;
using RabbitMQ.Client;
using System.Text;

namespace Producer.Api.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly RabbitConfiguration _config;
        public RabbitService(IOptions<RabbitConfiguration> config)
        {
            _config = config.Value;
        }
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                UserName = _config.Username,
                Password = _config.Password,
                HostName = _config.Host
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("Messages", false, false, false);

            var jsonPayload = JsonConvert.SerializeObject(message);
            var bytesPayload = Encoding.UTF8.GetBytes(jsonPayload);

            channel.BasicPublish("", "Messages", null, bytesPayload);
        }
    }
}
