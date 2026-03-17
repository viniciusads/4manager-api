using System;

namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class UserException : Exception
    {
        public UserException()
            : base("Usuário não encontrado.") { }

        public UserException(string message)
            : base(message) { }

        public UserException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
