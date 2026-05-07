using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProjetoBanco.Api.Consumers;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Enums;
using ProjetoBanco.Api.Messaging;
using ProjetoBanco.Api.Models;
using ProjetoBanco.Api.Services;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/projeto-banco-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("Oracle")));
}

builder.Services.AddScoped<ContratacaoService>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<ContratacaoConsumer>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ProjetoBanco.Api"))
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317");
            });
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Produtos.FirstOrDefault() is null)
    {
        db.Produtos.Add(new Emprestimo
        {
            Nome = "Empréstimo Pessoal",
            TipoProduto = TipoProduto.Emprestimo,
            ValorMaximo = 50000,
            TaxaJuros = 2.5m
        });

        db.Produtos.Add(new MaquinaDeCartao
        {
            Nome = "Máquina de Cartão",
            TipoProduto = TipoProduto.MaquinaDeCartao,
            TaxaMdr = 2.99m
        });

        db.SaveChanges();
    }
}

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program { }