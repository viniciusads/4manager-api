using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Domain.Entities
{
    public class Timesheet
    {
        public Guid TimesheetId { get; set; }
        public DateTime Date {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? TagId { get; set; }
        public string? BlockColor { get; set; } = string.Empty;
    }
}
