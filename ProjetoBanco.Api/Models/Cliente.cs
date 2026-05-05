using System.ComponentModel.DataAnnotations;

namespace ProjetoBanco.Api.Models;

public abstract class Cliente
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    public int AgenciaId { get; set; }
    public Agencia? Agencia { get; set; }

    public List<Contratacao> Contratacoes { get; set; } = new();
}