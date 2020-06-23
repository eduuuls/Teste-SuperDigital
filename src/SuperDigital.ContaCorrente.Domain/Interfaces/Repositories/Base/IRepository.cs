using SuperDigital.ContaCorrente.Domain.Entidades.Base;
using System;
using System.Linq;

namespace SuperDigital.ContaCorrente.Domain.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T: Entidade<T>
    {
        void Adicionar(T entidade);
        void Atualizar(T entidade);
        void Excluir(Guid id);
        void Excluir(T entidade);
        T Buscar(Guid id);
        IQueryable<T> Listar();

    }
}
