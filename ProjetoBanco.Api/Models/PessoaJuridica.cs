using System.ComponentModel.DataAnnotations;

namespace ProjetoBanco.Api.Models;

public class PessoaJuridica : Cliente
{
    [Required]
    [MaxLength(14)]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string RazaoSocial { get; set; } = string.Empty;
}