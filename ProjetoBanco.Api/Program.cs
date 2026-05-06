using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Consumers;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Enums;
using ProjetoBanco.Api.Messaging;
using ProjetoBanco.Api.Models;
using ProjetoBanco.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProjetoBancoDb"));

builder.Services.AddScoped<ContratacaoService>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<ContratacaoConsumer>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }