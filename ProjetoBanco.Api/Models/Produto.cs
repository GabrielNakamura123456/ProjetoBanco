using ProjetoBanco.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoBanco.Api.Models;

public abstract class Produto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public TipoProduto TipoProduto { get; set; }

    public List<Contratacao> Contratacoes { get; set; } = new();
}