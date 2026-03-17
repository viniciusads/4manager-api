namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class CustomerException : Exception
    {
        public CustomerException() : base("Cliente não encontrado."){}
        public CustomerException(string message) : base(message)
        {
        }
    }
}