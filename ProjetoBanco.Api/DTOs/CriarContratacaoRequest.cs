namespace ProjetoBanco.Api.DTOs;

public class CriarContratacaoRequest
{
    public int ClienteId { get; set; }

    public int ProdutoId { get; set; }

    public decimal ValorSolicitado { get; set; }

    public int ScoreCredito { get; set; }

    public decimal? FaturamentoMensal { get; set; }
}