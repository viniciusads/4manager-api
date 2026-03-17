using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services.Formatters
{
    public class CsvExportFormatter : IExportFormatter
    {
        public byte[] Format(IEnumerable<TimesheetExportDto> timesheets)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Descricao,Duracao,Membro,Projeto,Cliente,TipoDeAtividade,StartDate,StartTime,StopDate,StopTime");

            foreach (var t in timesheets)
            {
                sb.AppendLine(
                    $"\"{EscapeCsv(t.Description)}\"," +
                    $"\"{t.Duration}\"," +
                    $"\"{EscapeCsv(t.Member)}\"," +
                    $"\"{EscapeCsv(t.Project)}\"," +
                    $"\"{EscapeCsv(t.Customer)}\"," +
                    $"\"{EscapeCsv(t.ActivityType)}\"," +
                    $"{t.StartDate:dd-MM-yyyy}," +
                    $"{t.StartTime}," +
                    $"{t.StopDate:dd-MM-yyyy}," +
                    $"{t.StopTime}"
                );
            }

            var encoding = new UTF8Encoding(true);
            return encoding.GetBytes(sb.ToString());
        }

        public string GetContentType() => "text/csv";

        public string GetFileExtension() => "csv";

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("\"", "\"\"");
        }
    }
}