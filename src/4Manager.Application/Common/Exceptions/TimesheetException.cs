namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public class TimesheetException : Exception
    {
        public TimesheetException()
           : base("Timesheet não encontrado.") { }

        public TimesheetException(string message)
            : base(message) { }

        public TimesheetException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
