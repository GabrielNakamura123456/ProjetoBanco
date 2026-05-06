using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("pf")]
    public async Task<IActionResult> CriarPessoaFisica(PessoaFisica cliente)
    {
        var agencia = await _context.Agencias.FindAsync(cliente.AgenciaId);

        if (agencia is null)
            return BadRequest("Agência não encontrada.");

        var cpfExiste = _context.PessoasFisicas.FirstOrDefault(p => p.Cpf == cliente.Cpf);

        if (cpfExiste is not null)
            return BadRequest("CPF já cadastrado.");

        _context.PessoasFisicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }

    [HttpPost("pj")]
    public async Task<IActionResult> CriarPessoaJuridica(PessoaJuridica cliente)
    {
        var agencia = await _context.Agencias.FindAsync(cliente.AgenciaId);

        if (agencia is null)
            return BadRequest("Agência não encontrada.");

        var cnpjExiste = _context.PessoasJuridicas.FirstOrDefault(p => p.Cnpj == cliente.Cnpj);

        if (cnpjExiste is not null)
            return BadRequest("CNPJ já cadastrado.");

        _context.PessoasJuridicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Agencia)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente is null)
            return NotFound();

        return Ok(cliente);
    }
}