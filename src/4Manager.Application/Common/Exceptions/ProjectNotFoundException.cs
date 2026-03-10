using System;

namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException()
            : base("Projeto não encontrado.") { }

        public ProjectNotFoundException(string message)
            : base(message) { }

        public ProjectNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
