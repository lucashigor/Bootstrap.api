namespace Adasit.Bootstrap.Infrastructure.rabbitmq;

using Adasit.Bootstrap.Application.Dto.Models.Events;
using Adasit.Bootstrap.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

public class SendMessageRabbitmq : IMessageSenderInterface
{
    public SendMessageRabbitmq(IConfiguration configuration)
    {
        hostName = configuration["rabbitmq:hostName"];
        hostPort = configuration["rabbitmq:hostPort"];
        virtualHost = configuration["rabbitmq:virtualHost"];
        username = configuration["rabbitmq:username"];
        password = configuration["rabbitmq:password"];
    }

    private readonly string? hostName;
    private readonly string? hostPort;
    private readonly string? virtualHost;
    private readonly string? username;
    private readonly string? password;

    public Task Send(TopicNames name, object data)
    {
        var connectionFactory = new ConnectionFactory();

        if (!string.IsNullOrEmpty(hostName))
            connectionFactory.HostName = hostName;

        if (!string.IsNullOrEmpty(hostPort))
        {
            var port = int.Parse(hostPort);
            connectionFactory.Port = port;
        }

        if (!string.IsNullOrEmpty(virtualHost))
            connectionFactory.VirtualHost = virtualHost;

        if (!string.IsNullOrEmpty(username))
            connectionFactory.UserName = username;

        if (!string.IsNullOrEmpty(password))
            connectionFactory.Password = password;

        connectionFactory.DispatchConsumersAsync = true;

        var connection = connectionFactory.CreateConnection();

        var model = connection.CreateModel();

        var properties = model.CreateBasicProperties();

        properties.Persistent = false;

        byte[] messagebuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(data));

        model.BasicPublish(name.Value, "", properties, messagebuffer);

        return Task.CompletedTask;

    }
}
