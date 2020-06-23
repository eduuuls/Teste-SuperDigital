using SuperDigital.ContaCorrente.Domain.Entidades;
using System.Collections.Generic;

namespace SuperDigital.ContaCorrente.Domain.Interfaces.Services
{
    public interface IContaCorrenteService
    {
        List<Conta> Listar();
        void Incluir(Conta conta);
    }
}
