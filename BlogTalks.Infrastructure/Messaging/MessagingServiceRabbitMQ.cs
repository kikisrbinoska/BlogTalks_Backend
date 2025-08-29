using System.Text;
using BlogTalks.Application.Abstractions;
using BlogTalks.EmailSenderApi.DTO;
using BlogTalks.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class MessagingServiceRabbitMQ : IMessagingService
{
    private readonly RabbitMQSettings _rabbitMQSettings;    

    public MessagingServiceRabbitMQ(IOptions<RabbitMQSettings> rabbitMqOptions)
    {
        _rabbitMQSettings = rabbitMqOptions.Value;
    }

    public async Task Send(EmailDTO emailDto)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMQSettings.Host,
            UserName = _rabbitMQSettings.Username,
            Password = _rabbitMQSettings.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(_rabbitMQSettings.ExchangeName, _rabbitMQSettings.ExchangeType, durable: true);
        await channel.QueueDeclareAsync(_rabbitMQSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(_rabbitMQSettings.QueueName, _rabbitMQSettings.ExchangeName, _rabbitMQSettings.RouteKey);

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailDto));

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: _rabbitMQSettings.ExchangeName,
            routingKey: _rabbitMQSettings.RouteKey,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }
}