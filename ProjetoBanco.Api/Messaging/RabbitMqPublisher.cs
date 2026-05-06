using System.Text;
using System.Text.Json;
using ProjetoBanco.Api.DTOs;
using RabbitMQ.Client;

namespace ProjetoBanco.Api.Messaging;

public class RabbitMqPublisher
{
    private const string QueueName = "contratacao-solicitada";

    public void Publicar(ContratacaoMessage message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();

        properties.Persistent = true;

        channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: properties,
            body: body);
    }
}