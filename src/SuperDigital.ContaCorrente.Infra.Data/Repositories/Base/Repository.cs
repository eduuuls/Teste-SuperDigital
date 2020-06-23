using SuperDigital.ContaCorrente.Domain.Entidades.Base;
using SuperDigital.ContaCorrente.Domain.Interfaces.Repositories.Base;
using SuperDigital.ContaCorrente.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SuperDigital.ContaCorrente.Infra.Data.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : Entidade<T>
    {
        readonly protected ContaCorrenteContext<T> _context;

        public Repository(ContaCorrenteContext<T> context)
        {
            _context = context;

            if (_context.Entidades == null)
                _context.Entidades = new List<T>();
        }
        public void Adicionar(T entidade)
        {
            _context.Entidades.Add(entidade);
        }

        public void Atualizar(T entidade)
        {
           var entidadeAtual = _context.Entidades.Find(e => e.Id == entidade.Id);

            entidadeAtual = entidade;
        }

        public T Buscar(Guid id)
        {
            var entidade = _context.Entidades.Find(e=> e.Id == id);
            return entidade;
        }

        public void Excluir(Guid id)
        {
            Excluir(Buscar(id));
        }

        public void Excluir(T entidade)
        {
            _context.Entidades.Remove(entidade);
        }

        public IQueryable<T> Listar()
        {
            return _context.Entidades.AsQueryable();
        }
    }
}
