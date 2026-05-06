using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.DTOs;
using ProjetoBanco.Api.Models;
using ProjetoBanco.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProjetoBanco.Api.Consumers;

public class ContratacaoConsumer : BackgroundService
{
    private const string QueueName = "contratacao-solicitada";

    private readonly IServiceScopeFactory _scopeFactory;

    public ContratacaoConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var service = scope.ServiceProvider.GetRequiredService<ContratacaoService>();

            var body = ea.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            var message = JsonSerializer.Deserialize<ContratacaoMessage>(json);

            if (message is null)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            var contratacao = db.Contratacoes.Find(message.ContratacaoId);

            if (contratacao is null)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            var produto = db.Produtos.Find(message.ProdutoId);

            if (produto is null)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            service.Processar(contratacao, produto);

            db.SaveChanges();

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(
            queue: QueueName,
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }
}