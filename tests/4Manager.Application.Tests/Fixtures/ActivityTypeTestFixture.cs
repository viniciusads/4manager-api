using _4Tech._4Manager.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class ActivityTypeTestFixture
    {
        private readonly Faker<ActivityType> _ActivityTypeEntityFaker;

        public ActivityTypeTestFixture()
        {
            _ActivityTypeEntityFaker = new Faker<ActivityType>()
                .RuleFor(t => t.ActivityTypeId, f => f.Random.Guid())
                .RuleFor(t => t.ActivityTypeName, f => f.Name.FullName())
                .RuleFor(t => t.ActivityTypeColor, f => f.Name.FullName())
                .RuleFor(t => t.IsActive, f => f.Random.Bool());
        }

        public List<ActivityType> GeneratesActivityType(int count)
        {
            return _ActivityTypeEntityFaker.Generate(count);
        }
    }
}
