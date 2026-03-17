using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services.Formatters
{
    public class ExportFormatterFactory
    {
        public static IExportFormatter CreateFormatter(string format)
        {
            return format.ToLower() switch
            {
                "txt" => new TxtExportFormatter(),
                "csv" => new CsvExportFormatter(),
                _ => throw new ArgumentException($"Formato não suportado: {format}")
            };
        }
    }
}