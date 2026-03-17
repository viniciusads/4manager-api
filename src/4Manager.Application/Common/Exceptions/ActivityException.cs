namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class ActivityException : Exception
    {
        public ActivityException()
            : base("Atividade não encontrada") { }

        public ActivityException(string message)
            : base(message) { }

        public ActivityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
