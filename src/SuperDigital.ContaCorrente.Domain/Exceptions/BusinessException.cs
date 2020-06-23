using SuperDigital.ContaCorrente.Domain.Enums;
using System;

namespace SuperDigital.ContaCorrente.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        private EBusinessErrors _code;
        private string _message;

        public BusinessException(EBusinessErrors Code, string Message)
        {
            _code = Code;
            _message = Message;
        }

        public EBusinessErrors Code => _code;
        public override string Message => _message;
    }
}
