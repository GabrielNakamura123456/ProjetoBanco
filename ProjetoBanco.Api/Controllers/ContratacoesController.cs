using Microsoft.AspNetCore.Mvc;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.DTOs;
using ProjetoBanco.Api.Enums;
using ProjetoBanco.Api.Messaging;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RabbitMqPublisher _publisher;

    public ContratacoesController(AppDbContext context, RabbitMqPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> Solicitar(CriarContratacaoRequest request)
    {
        var cliente = await _context.Clientes.FindAsync(request.ClienteId);

        if (cliente is null)
            return NotFound("Cliente não encontrado.");

        var produto = await _context.Produtos.FindAsync(request.ProdutoId);

        if (produto is null)
            return NotFound("Produto não encontrado.");

        var contratacao = new Contratacao
        {
            ClienteId = request.ClienteId,
            ProdutoId = request.ProdutoId,
            ValorSolicitado = request.ValorSolicitado,
            ScoreCredito = request.ScoreCredito,
            FaturamentoMensal = request.FaturamentoMensal,
            Status = StatusContratacao.Pendente,
            CriadoEm = DateTime.UtcNow
        };

        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync();

        var message = new ContratacaoMessage
        {
            ContratacaoId = contratacao.Id,
            ClienteId = contratacao.ClienteId,
            ProdutoId = contratacao.ProdutoId,
            ValorSolicitado = contratacao.ValorSolicitado,
            ScoreCredito = contratacao.ScoreCredito,
            FaturamentoMensal = contratacao.FaturamentoMensal
        };

        _publisher.Publicar(message);

        return AcceptedAtAction(nameof(BuscarPorId), new { id = contratacao.Id }, contratacao);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var contratacao = await _context.Contratacoes.FindAsync(id);

        if (contratacao is null)
            return NotFound();

        return Ok(contratacao);
    }
}