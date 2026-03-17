namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class GuidException : Exception
    {
        public GuidException() 
            : base("O Guid solicitado não se encontra no banco de dados.") { }
    }
}
