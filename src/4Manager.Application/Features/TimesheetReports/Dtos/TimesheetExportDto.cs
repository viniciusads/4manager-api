using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class TimesheetExportDto
    {
        public string Description { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Member { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public DateTime StopDate { get; set; }
        public string StopTime { get; set; } = string.Empty;
    }
    public class TimesheetExportInternalDto : TimesheetExportDto
    {
        public string? ProjectId { get; set; }
        public string BlockColor { get; set; } = "#3B5C7E";
    }

}