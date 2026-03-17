using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Helpers
{
    public static class HtmlHelper
    {
        public static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return HttpUtility.HtmlEncode(value);
        }
        public static string ToJsonSafe(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
    }
}
