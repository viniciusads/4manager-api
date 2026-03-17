namespace _4Tech._4Manager.Application.Common.Date
{
    public static class FormatTimeHelper
    {
        public static string FormatTime(this TimeSpan time)
        {
            return time.ToString(@"hh\:mm\:ss");
        }
    }
}
