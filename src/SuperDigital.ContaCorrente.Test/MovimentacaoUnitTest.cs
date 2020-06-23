using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories;
using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.API.Controllers;
using SuperDigital.ContaCorrente.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq;
using AutoMapper;
using Bogus;
using System.Linq;
using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using SuperDigital.ContaCorrente.Domain.Exceptions;

namespace SuperDigital.ContaCorrente.Test.Unitario
{
    public sealed class MovimentacaoUnitTest
    {
        #region Objetos
        private MovimentacaoViewModel movimentacaoErroModel;
        private MovimentacaoViewModel movimentacaoModel;
        private ContaCorrenteViewModel contaCorrenteOrigemModel;
        private ContaCorrenteViewModel contaCorrenteDestinoModel;
        private Conta contaCorrenteOrigem;
        private Conta contaCorrenteDestino;
        private List<Conta> contas;
        #endregion

        #region Servicos
        readonly Mock<IMovimentacaoService> _movimentacaoService;
        #endregion

        #region Repositorios
        readonly Mock<IContaCorrenteRepository> _contaCorrenteRepository;
        #endregion

        #region Controllers
        readonly MovimentacaoController _movimentacaoController;
        #endregion

        readonly Mock<IMapper> _mapper;
        readonly Mock<IUnitOfWork> _unitOfWork;
        public MovimentacaoUnitTest()
        {
            //Instânciando dependências 
            _movimentacaoService = new Mock<IMovimentacaoService>();
            _mapper = new Mock<IMapper>();
            _contaCorrenteRepository = new Mock<IContaCorrenteRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            InicializarObjetos();

            _movimentacaoController = new MovimentacaoController(_mapper.Object, _movimentacaoService.Object);
        }

        [Fact]
        public void DeveRetornarOk()
        {
            _mapper.Setup(x => x.Map<Conta>(contaCorrenteOrigemModel))
                .Returns(contaCorrenteOrigem);

            _mapper.Setup(x => x.Map<Conta>(contaCorrenteDestinoModel))
                 .Returns(contaCorrenteDestino);

            var result = (OkObjectResult)_movimentacaoController.Post(movimentacaoModel);

            Assert.Equal(StatusCodes.Status200OK,result.StatusCode);
        }

        [Fact]
        public void DeveRetornarBadRequest()
        {
            _mapper.Setup(x => x.Map<Conta>(contaCorrenteOrigemModel))
                .Returns(contaCorrenteOrigem);

            _mapper.Setup(x => x.Map<Conta>(contaCorrenteDestinoModel))
                 .Returns(contaCorrenteDestino);

            var result = (ObjectResult)_movimentacaoController.Post(movimentacaoErroModel);

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }


        private void InicializarObjetos()
        {
            //Dados fake
            contaCorrenteOrigemModel = new Faker<ContaCorrenteViewModel>()
                .RuleFor(c => c.CodigoAgencia, f => f.Random.Number(1, 10))
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.NumeroConta, f => f.Random.Number(10000, 20000))
                .RuleFor(c => c.Saldo, f => f.Random.Decimal(100, 50000));

            contaCorrenteDestinoModel = new Faker<ContaCorrenteViewModel>()
                .RuleFor(c => c.CodigoAgencia, f => f.Random.Number(1, 10))
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.NumeroConta, f => f.Random.Number(10000, 20000))
                .RuleFor(c => c.Saldo, f => 1000);

            movimentacaoModel = new Faker<MovimentacaoViewModel>()
                .RuleFor(x => x.ContaDestino, contaCorrenteDestinoModel)
                .RuleFor(x => x.ContaOrigem, contaCorrenteOrigemModel)
                .RuleFor(x => x.Valor, f => f.Random.Decimal(10, 50));

            movimentacaoErroModel = new Faker<MovimentacaoViewModel>()
                .RuleFor(x => x.ContaDestino, contaCorrenteDestinoModel)
                .RuleFor(x => x.ContaOrigem, contaCorrenteOrigemModel)
                .RuleFor(x => x.Valor, 0);

            contaCorrenteOrigem = new Faker<Conta>()
               .RuleFor(x => x.CodigoAgencia, contaCorrenteOrigemModel.CodigoAgencia)
               .RuleFor(x => x.Id, contaCorrenteOrigemModel.Id)
               .RuleFor(x => x.NumeroConta, contaCorrenteOrigemModel.NumeroConta)
               .RuleFor(x => x.Saldo, contaCorrenteOrigemModel.Saldo);

            contaCorrenteDestino = new Faker<Conta>()
                .RuleFor(x => x.CodigoAgencia, contaCorrenteDestinoModel.CodigoAgencia)
                .RuleFor(x => x.Id, contaCorrenteDestinoModel.Id)
                .RuleFor(x => x.NumeroConta, contaCorrenteDestinoModel.NumeroConta)
                .RuleFor(x => x.Saldo, contaCorrenteDestinoModel.Saldo);

            contas = new List<Conta>() { contaCorrenteOrigem, contaCorrenteDestino };


            _contaCorrenteRepository.Setup(r => r.Listar()).Returns(contas.AsQueryable());
            _unitOfWork.Setup(r => r.Commit());
            _contaCorrenteRepository.Setup(r => r.Adicionar(contaCorrenteOrigem));
            _contaCorrenteRepository.Setup(r => r.Adicionar(contaCorrenteDestino));
            _movimentacaoService.Setup(s => s.ExecutarMovimentacao(contaCorrenteOrigem, contaCorrenteDestino, movimentacaoModel.Valor)).Returns(1100);
            _movimentacaoService.Setup(s => s.ExecutarMovimentacao(contaCorrenteOrigem, contaCorrenteDestino, 0)).Throws(new BusinessException(Domain.Enums.EBusinessErrors.SaldoInsuficiente,"Erro"));
        }
    }
}
