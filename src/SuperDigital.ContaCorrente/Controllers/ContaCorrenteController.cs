using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SuperDigital.ContaCorrente.API.ViewModels;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using System.Collections.Generic;
using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Exceptions;

namespace SuperDigital.ContaCorrente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCorrenteController : Controller
    {
        readonly IContaCorrenteService _contaCorrenteService;
        readonly IMapper _mapper;

        public ContaCorrenteController(IMapper mapper,
                                        IContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todas as contas correntes.
        /// </summary>
        /// <response code="200">Caso sucesso</response>
        /// <response code="500">Caso ocorra erro técnico</response>
        [HttpGet]
        [Route("Listar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            try
            {
               var result = _contaCorrenteService.Listar();

                var contas = _mapper.Map<List<ContaCorrenteViewModel>>(result);

                return Ok(new
                {
                    sucesso = true,
                    dados = contas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    sucesso = false,
                    erros = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message
                    }
                });
            }
        }

        /// <summary>
        /// Executa transferência de valores entre contas.
        /// </summary>
        /// <remarks>
        /// <para>Na primeira execução da aplicação as contas não existem.
        /// Neste caso elas deverão ser cadastradas e o valor informado no campo "saldo" será utilizado como saldo inicial. 
        /// </para>
        /// Exemplo de request:
        /// 
        ///     POST
        ///     {
        ///        {
        ///          "codigoAgencia": 0001,
        ///          "numeroConta": 11111,
        ///          "saldo": 300
        ///        }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Caso sucesso</response>
        /// <response code="400">Caso ocorra erro de negócio</response>
        /// <response code="500">Caso ocorra erro técnico</response>
        [HttpPost]
        [Route("Incluir")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] ContaCorrenteViewModel model)
        {
            try
            {
                var conta = _mapper.Map<Conta>(model);

                _contaCorrenteService.Incluir(conta);

                return Ok(new
                {
                    sucesso = true
                });
            }
            catch (BusinessException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    sucesso = false,
                    erros = new
                    {
                        Type = "BusinessException",
                        Code = ex.Code,
                        Message = ex.Message
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    sucesso = false,
                    erros = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message
                    }
                });
            }
        }
    }
}
