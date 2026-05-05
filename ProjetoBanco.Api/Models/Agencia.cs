using System.ComponentModel.DataAnnotations;

namespace ProjetoBanco.Api.Models;

public class Agencia
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Numero { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public List<Cliente> Clientes { get; set; } = new();
}