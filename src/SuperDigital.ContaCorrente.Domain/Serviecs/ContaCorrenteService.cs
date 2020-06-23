using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Enums;
using SuperDigital.ContaCorrente.Domain.Exceptions;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using SuperDigital.ContaCorrente.Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperDigital.ContaCorrente.Domain.Services
{
    public sealed class ContaCorrenteService : Service, IContaCorrenteService
    {
        readonly IContaCorrenteRepository _contaCorrenteRepository;
        public ContaCorrenteService(IContaCorrenteRepository contaCorrenteRepository,
                                                        IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public List<Conta> Listar()
        {
            return _contaCorrenteRepository.Listar().ToList();
        }

        public void Incluir(Conta conta)
        {
            //Caso não exista a conta, é feito o cadastro.
            if (!ValidarContaExistente(conta))
            {
                var id = System.Guid.NewGuid();

                conta.Id = id;
                _contaCorrenteRepository.Adicionar(conta);
                _unitOfWork.Commit();
            }
            else
                throw new BusinessException(EBusinessErrors.ContaJaCadastrada, $"A conta {conta.NumeroConta} já está cadastrada.");

        }

        private bool ValidarContaExistente(Conta contaCorrente)
        {
            return _contaCorrenteRepository.Listar().Any(c => c.NumeroConta == contaCorrente.NumeroConta);
        }
    }
}
