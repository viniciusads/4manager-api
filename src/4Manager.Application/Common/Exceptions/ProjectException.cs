using System;

namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class ProjectException : Exception
    {
        public ProjectException()
            : base("Projeto não encontrado.") { }

        public ProjectException(string message)
            : base(message) { }

        public ProjectException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
