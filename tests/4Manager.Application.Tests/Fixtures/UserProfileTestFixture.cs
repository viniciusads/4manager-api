using _4Tech._4Manager.Application.Features.Team.Dtos;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using Bogus;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class UserProfileTestFixture
    {
        private readonly Faker<UserProfile> _userEntityFaker;
        public UserProfileTestFixture()
        {
            _userEntityFaker = new Faker<UserProfile>()
                .RuleFor(u => u.UserId, f => f.Random.Guid())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Function, f => f.PickRandom<RoleEnum>())
                .RuleFor(u => u.IsActive, f => f.Random.Bool())
                .RuleFor(u => u.Position, f => f.PickRandom<PositionEnum>())
                .RuleFor(u => u.UserProfilePicture, f => f.Name.FullName());
        }

        public List<UserProfile> GenerateUserProfile(int count)
        {
            return _userEntityFaker.Generate(count);
        }
    }
}
