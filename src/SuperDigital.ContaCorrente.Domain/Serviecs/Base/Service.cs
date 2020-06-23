using SuperDigital.ContaCorrente.Domain.Interfaces.UnitOfWork;
using System;

namespace SuperDigital.ContaCorrente.Domain.Services.Base
{
    public abstract class Service
    {
        protected IUnitOfWork _unitOfWork;

        protected Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected bool Commit(string mensagemErroCommit)
        {
            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                //TODO: pode ser implementado log aqui.
                return false;
            }

            return true;
        }
    }
}
