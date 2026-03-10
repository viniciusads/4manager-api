namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message)
        {
        }
    }
}