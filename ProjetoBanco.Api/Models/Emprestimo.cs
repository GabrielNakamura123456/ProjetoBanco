namespace ProjetoBanco.Api.Models;

public class Emprestimo : Produto
{
    public decimal ValorMaximo { get; set; }
    public decimal TaxaJuros { get; set; }
}