namespace ProjetoBanco.Api.DTOs;

public class ContratacaoMessage
{
    public int ContratacaoId { get; set; }

    public int ClienteId { get; set; }

    public int ProdutoId { get; set; }

    public decimal ValorSolicitado { get; set; }

    public int ScoreCredito { get; set; }

    public decimal? FaturamentoMensal { get; set; }
}