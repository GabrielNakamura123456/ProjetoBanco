using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Data;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Controllers;

[ApiController]
[Route("api/agencias")]
public class AgenciasController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgenciasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Agencia agencia)
    {
        _context.Agencias.Add(agencia);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = agencia.Id }, agencia);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var agencia = await _context.Agencias.FindAsync(id);

        if (agencia is null)
        {
            return NotFound();
        }

        return Ok(agencia);
    }
}