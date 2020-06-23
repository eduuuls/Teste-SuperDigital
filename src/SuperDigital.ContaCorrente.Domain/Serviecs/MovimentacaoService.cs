using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Enums;
using SuperDigital.ContaCorrente.Domain.Exceptions;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using SuperDigital.ContaCorrente.Domain.Services.Base;
using System;
using System.Linq;

namespace SuperDigital.ContaCorrente.Domain.Services
{
    public sealed class MovimentacaoService : Service, IMovimentacaoService
    {
        readonly IContaCorrenteRepository _contaCorrenteRepository;
        const string MSG_SALDO_INSUFICIENTE = "Não foi possível efetuar o débido de {0:C} na conta {1}: Saldo insuficiente.";
        public MovimentacaoService(IContaCorrenteRepository contaCorrenteRepository,
                                                        IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public decimal ExecutarMovimentacao(Conta origem, Conta destino, decimal valor)
        {
            var contaDebito = _contaCorrenteRepository.Listar()
                                    .FirstOrDefault(c => c.NumeroConta == origem.NumeroConta);

            if (contaDebito == null)
                throw new BusinessException(EBusinessErrors.ContaInexistente, $"A conta origem {origem.NumeroConta} é inválida.");

            var contaCredito = _contaCorrenteRepository.Listar()
                                    .FirstOrDefault(c => c.NumeroConta == destino.NumeroConta);

            if (contaCredito == null)
                throw new BusinessException(EBusinessErrors.ContaInexistente, $"A conta destino {destino.NumeroConta} é inválida.");

            EfetuarLancamento(contaDebito, ETipoLancamento.Debito, valor);

            EfetuarLancamento(contaCredito, ETipoLancamento.Credito, valor);

            //Grava as alterações
            _unitOfWork.Commit();

            return contaCredito.Saldo;
        }

        private  void EfetuarLancamento(Conta contaCorrente, ETipoLancamento eTipoLancamento, decimal valor)
        {
            switch (eTipoLancamento)
            {
                case ETipoLancamento.Debito:

                    if (contaCorrente.Saldo < valor)
                    {
                        var msg = string.Format(MSG_SALDO_INSUFICIENTE, valor, contaCorrente.NumeroConta);
                        throw new BusinessException(EBusinessErrors.SaldoInsuficiente, msg);
                    }

                    contaCorrente.Saldo -= valor;
                    break;
                case ETipoLancamento.Credito:
                    contaCorrente.Saldo += valor;
                    break;
                default:
                    break;
            }
        }

        private Conta ObterDadosConta(Conta conta)
        {
           return _contaCorrenteRepository.Listar()
                                    .FirstOrDefault(c => c.NumeroConta == conta.NumeroConta);
        }
    }
}
