using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories;
using SuperDigital.ContaCorrente.Infra.Data.Context;

namespace SuperDigital.ContaCorrente.Infra.Data.Repositories
{
    public sealed class ContaCorrenteRepository: Base.Repository<Conta>, IContaCorrenteRepository
    {
        public ContaCorrenteRepository(ContaCorrenteContext<Conta> context) : base(context)
        {

        }
    }
}
