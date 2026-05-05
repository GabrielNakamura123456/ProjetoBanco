using ProjetoBanco.Api.Enums;

namespace ProjetoBanco.Api.Models;

public class Contratacao
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public StatusContratacao Status { get; set; } = StatusContratacao.Pendente;

    public decimal ValorSolicitado { get; set; }

    public int ScoreCredito { get; set; }

    public decimal? FaturamentoMensal { get; set; }

    public decimal? TaxaAplicada { get; set; }

    public string? MotivoRecusa { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessadoEm { get; set; }
}