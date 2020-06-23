using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SuperDigital.ContaCorrente.API.ViewModels;
using SuperDigital.ContaCorrente.Domain.Interfaces.Services;
using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
namespace SuperDigital.ContaCorrente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : Controller
    {
        readonly IMovimentacaoService _movimentacaoService;
        readonly IMapper _mapper;

        public MovimentacaoController(IMapper mapper,
                                        IMovimentacaoService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Executa transferência de valores entre contas.
        /// </summary>
        /// <response code="200">Caso sucesso</response>
        /// <response code="400">Caso ocorra erro de negócio</response>
        /// <response code="500">Caso ocorra erro técnico</response>
        [HttpPost]
        [Route("ExecutarMovimentacao")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] MovimentacaoViewModel model)
        {
            try
            {
                var contaOrigem = _mapper.Map<Conta>(model.ContaOrigem);
                var contaDestino = _mapper.Map<Conta>(model.ContaDestino);

                var resultado = _movimentacaoService.ExecutarMovimentacao(contaOrigem, contaDestino, model.Valor);

                return Ok(new
                {
                    saldo = resultado,
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
