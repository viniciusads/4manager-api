using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class ProjectBreakdownDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectColor { get; set; } = "#3B5C7E";
        public string Duration { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public List<DescriptionItemDto> Items { get; set; } = new();
    }

    public class DescriptionItemDto
    {
        public string Description { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public double Percentage { get; set; }
    }
}
