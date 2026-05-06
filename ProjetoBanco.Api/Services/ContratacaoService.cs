using ProjetoBanco.Api.Enums;
using ProjetoBanco.Api.Models;

namespace ProjetoBanco.Api.Services;

public class ContratacaoService
{
    public void Processar(Contratacao contratacao, Produto produto)
    {
        if (produto is Emprestimo)
        {
            ProcessarEmprestimo(contratacao);
            return;
        }

        if (produto is MaquinaDeCartao)
        {
            ProcessarMaquinaDeCartao(contratacao);
            return;
        }

        contratacao.Status = StatusContratacao.Recusada;
        contratacao.MotivoRecusa = "Produto não implementado.";
        contratacao.ProcessadoEm = DateTime.UtcNow;
    }

    private void ProcessarEmprestimo(Contratacao contratacao)
    {
        decimal limite = contratacao.ScoreCredito switch
        {
            >= 800 => 50000,
            >= 600 => 20000,
            >= 400 => 5000,
            _ => 0
        };

        if (limite > 0 && contratacao.ValorSolicitado <= limite)
        {
            contratacao.Status = StatusContratacao.Aprovada;
        }
        else
        {
            contratacao.Status = StatusContratacao.Recusada;
            contratacao.MotivoRecusa = "Valor solicitado acima do limite aprovado pelo score.";
        }

        contratacao.ProcessadoEm = DateTime.UtcNow;
    }

    private void ProcessarMaquinaDeCartao(Contratacao contratacao)
    {
        var faturamento = contratacao.FaturamentoMensal ?? 0;

        if (faturamento < 1000)
        {
            contratacao.Status = StatusContratacao.Recusada;
            contratacao.MotivoRecusa = "Faturamento mensal insuficiente.";
            contratacao.ProcessadoEm = DateTime.UtcNow;
            return;
        }

        contratacao.Status = StatusContratacao.Aprovada;
        contratacao.TaxaAplicada = faturamento switch
        {
            >= 10000 => 1.99m,
            >= 5000 => 2.49m,
            _ => 2.99m
        };

        contratacao.ProcessadoEm = DateTime.UtcNow;
    }
}