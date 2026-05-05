using System.ComponentModel.DataAnnotations;

namespace ProjetoBanco.Api.Models;

public class PessoaFisica : Cliente
{
    [Required]
    [MaxLength(11)]
    public string Cpf { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }
}