using Consumer.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.Api.Services
{
    public class RabbitListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitConfiguration _conf;
        public RabbitListener(IOptions<RabbitConfiguration> conf)
        {
            _conf = conf.Value;

            var factory = new ConnectionFactory { HostName = _conf.Host, UserName = _conf.Username, Password = _conf.Password };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "Messages", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (o, arguments) =>
            {
                var content = Encoding.UTF8.GetString(arguments.Body.ToArray());

                var message = JsonConvert.DeserializeObject<Message>(content);

                Console.WriteLine(message);

                _channel.BasicAck(arguments.DeliveryTag, false);
            };

            _channel.BasicConsume("Messages", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
