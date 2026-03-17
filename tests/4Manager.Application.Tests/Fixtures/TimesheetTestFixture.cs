using _4Tech._4Manager.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class TimesheetTestFixture
    {
        private readonly Faker<Timesheet> _timesheetEntityFaker;

        public TimesheetTestFixture()
        {
            _timesheetEntityFaker = new Faker<Timesheet>()
                .RuleFor(t => t.TimesheetId, f => f.Random.Guid())
                .RuleFor(t => t.Date, f => f.Date.Future(1))
                .RuleFor(t => t.StartDate, f => f.Date.Future(1))
                .RuleFor(t => t.EndDate, f => f.Date.Future(2))
                .RuleFor(t => t.Description, f => f.Name.FullName())
                .RuleFor(t => t.UserId, f => f.Random.Guid())
                .RuleFor(t => t.ActivityTypeId, f => f.Random.Guid());
        }

        public List<Timesheet> GeneratesTimesheet(int count){
                return _timesheetEntityFaker.Generate(count);
        }
    }
}
