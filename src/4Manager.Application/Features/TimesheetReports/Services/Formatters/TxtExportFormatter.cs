using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services.Formatters
{
    public class TxtExportFormatter : IExportFormatter
    {
        public byte[] Format(IEnumerable<TimesheetExportDto> timesheets)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Descricao|Duracao|Membro|Projeto|Cliente|TipoDeAtividade|StartDate|StartTime|StopDate|StopTime");

            foreach (var t in timesheets)
            {
                sb.AppendLine(
                    $"{t.Description}|" +
                    $"{t.Duration}|" +
                    $"{t.Member}|" +
                    $"{t.Project}|" +
                    $"{t.Customer}|" +
                    $"{t.ActivityType}|" +
                    $"{t.StartDate:dd-MM-yyyy}|" +
                    $"{t.StartTime}|" +
                    $"{t.StopDate:dd-MM-yyyy}|" +
                    $"{t.StopTime}"
                );
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public string GetContentType() => "text/plain";

        public string GetFileExtension() => "txt";
    }
}
