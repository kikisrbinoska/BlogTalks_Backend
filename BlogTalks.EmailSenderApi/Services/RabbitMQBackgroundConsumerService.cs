using System.Text;
using BlogTalks.EmailSenderApi.Data;
using BlogTalks.EmailSenderApi.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlogTalks.EmailSenderApi.Services
{
    public class RabbitMQBackgroundConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMQSettingseEmailSender _rabbitMQSettings;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQBackgroundConsumerService(IServiceScopeFactory serviceScopeFactory, RabbitMQSettingseEmailSender rabbitMQSettings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitMQSettings = rabbitMQSettings;
        }

        private async Task InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Hostname,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(_rabbitMQSettings.ExchangeName, _rabbitMQSettings.ExchhangeType, durable: true);
            await _channel.QueueDeclareAsync(_rabbitMQSettings.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(_rabbitMQSettings.QueueName, _rabbitMQSettings.ExchangeName, _rabbitMQSettings.RouteKey);
            await _channel.BasicQosAsync(0, 1, false);

            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            stoppingToken.ThrowIfCancellationRequested(); 
            
            await InitRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var emailDto = JsonConvert.DeserializeObject<EmailDTO>(message);

                await _emailSender.Send(emailDto);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            consumer.ShutdownAsync += OnConsumerShutdown;
            consumer.RegisteredAsync += OnConsumerRegistered;
            consumer.UnregisteredAsync += OnConsumerUnregistered;

            await _channel.BasicConsumeAsync(_rabbitMQSettings.QueueName, autoAck: false, consumer);
        }
        public override void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
            base.Dispose();
        }

        private Task OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerUnregistered(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerRegistered(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerShutdown(object sender, ShutdownEventArgs e) => Task.CompletedTask;
        private Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) => Task.CompletedTask;
    }
}
