using SuperDigital.ContaCorrente.Domain.Entidades;
using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using SuperDigital.ContaCorrente.Infra.Data.Context;

namespace SuperDigital.ContaCorrente.Infra.Data.UoW
{
    public class UnitOfWorkContaCorrente : IUnitOfWork
    {
        protected ContaCorrenteContext<Conta> _context;

        public UnitOfWorkContaCorrente(ContaCorrenteContext<Conta> context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}