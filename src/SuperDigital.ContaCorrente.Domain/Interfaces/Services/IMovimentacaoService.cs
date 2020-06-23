using SuperDigital.ContaCorrente.Domain.Entidades;

namespace SuperDigital.ContaCorrente.Domain.Interfaces.Services
{
    public interface IMovimentacaoService
    {
        decimal ExecutarMovimentacao(Conta origem, Conta destino, decimal valor);

    }
}
