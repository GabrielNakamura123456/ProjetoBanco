using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Consumers;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Messaging;
using ProjetoBanco.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProjetoBancoDb"));

builder.Services.AddScoped<ContratacaoService>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<ContratacaoConsumer>();

var app = builder.Build();

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