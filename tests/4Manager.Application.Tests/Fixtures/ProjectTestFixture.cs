using _4Tech._4Manager.Application.Features.Team.Dtos;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using Bogus;

namespace _4Tech._4Manager.Application.Tests.Fixtures
{
    public class ProjectTestFixture
    {
        private readonly Faker<User> _userEntityFaker;
        private readonly Faker<TeamCollaborator> _teamCollaboratorFaker;
        private readonly Faker<MemberResponseDto> _memberDtoFaker;
        private readonly Faker<TeamResponseDto> _teamDtoFaker;
        private readonly Faker<Team> _teamEntityFaker;
        private readonly Faker<Project> _projectEntityFaker;

        public ProjectTestFixture()
        {
            _userEntityFaker = new Faker<User>()
                .RuleFor(u => u.UserId, f => f.Random.Guid())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Role, f => f.PickRandom<RoleEnum>())
                .RuleFor(u => u.IsActive, f => f.Random.Bool());

            _teamCollaboratorFaker = new Faker<TeamCollaborator>()
                .RuleFor(tc => tc.TeamId, f => Guid.NewGuid())
                .RuleFor(tc => tc.CollaboratorId, f => f.Random.Guid())
                .RuleFor(tc => tc.User, f => _userEntityFaker.Generate());

            _memberDtoFaker = new Faker<MemberResponseDto>()
                .RuleFor(m => m.TeamCollaborator, f => _teamCollaboratorFaker.Generate(f.Random.Int(3, 5)));

            _teamDtoFaker = new Faker<TeamResponseDto>()
                .RuleFor(t => t.TeamId, f => f.Random.Guid())
                .RuleFor(t => t.ProjectName, f => f.Commerce.ProductName())
                .RuleFor(t => t.Members, f => _memberDtoFaker.Generate());

            _teamEntityFaker = new Faker<Team>()
                .RuleFor(t => t.TeamId, f => f.Random.Guid())
                .RuleFor(t => t.ProjectId, f => f.Random.Guid())
                .RuleFor(t => t.ManagerId, f => f.Random.Guid())
                .RuleFor(t => t.Manager, f => _userEntityFaker.Generate())
                .RuleFor(t => t.Collaborators, f => _teamCollaboratorFaker.Generate(f.Random.Int(3, 5)));

            _projectEntityFaker = new Faker<Project>()
                .RuleFor(p => p.ProjectId, f => f.Random.Guid())
                .RuleFor(p => p.ProjectName, f => f.Commerce.ProductName())
                .RuleFor(p => p.CustomerId, f => f.Random.Guid())
                .RuleFor(p => p.CustomerManagerId, f => f.Random.Guid())
                .RuleFor(p => p.StatusProject, f => f.PickRandom<ProjectStatusEnum>())
                .RuleFor(p => p.StartDate, f => f.Date.Future(1))
                .RuleFor(p => p.DeliveryDate, f => f.Date.Future(2))
                .RuleFor(p => p.TitleColor, f => f.Internet.Color())
                .RuleFor(p => p.StatusTime, f => f.Date.Recent(10))
                .RuleFor(p => p.Favorite, f => f.Random.Bool())
                .RuleFor(p => p.Archived, f => f.Random.Bool(0.1f))
                .RuleFor(p => p.Team, f => _teamEntityFaker.Generate())
                .RuleFor(p => p.Tickets, f => new List<Ticket>());
        }

        public List<Project> GenerateTestProjects(int count)
        {
            return _projectEntityFaker.Generate(count);
        }
    }
}
