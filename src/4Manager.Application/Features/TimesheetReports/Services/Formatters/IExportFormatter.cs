using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services.Formatters
{
    public interface IExportFormatter
    {
        byte[] Format(IEnumerable<TimesheetExportDto> timesheets);

        string GetContentType();

        string GetFileExtension();
    }
}
