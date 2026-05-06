using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Api.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjetoBanco.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Agencia> Agencias => Set<Agencia>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();
    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<MaquinaDeCartao> MaquinasDeCartao => Set<MaquinaDeCartao>();
    public DbSet<ReceberSalario> ReceberSalarios => Set<ReceberSalario>();

    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>()
            .HasDiscriminator<string>("TipoCliente")
            .HasValue<PessoaFisica>("PF")
            .HasValue<PessoaJuridica>("PJ");

        modelBuilder.Entity<Produto>()
            .HasDiscriminator<string>("TipoProdutoDiscriminator")
            .HasValue<Emprestimo>("EMPRESTIMO")
            .HasValue<MaquinaDeCartao>("MAQUINA")
            .HasValue<ReceberSalario>("SALARIO");

        modelBuilder.Entity<PessoaFisica>()
            .HasIndex(p => p.Cpf)
            .IsUnique();

        modelBuilder.Entity<PessoaJuridica>()
            .HasIndex(p => p.Cnpj)
            .IsUnique();
    }
}