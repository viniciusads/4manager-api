

using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class GetPagedProjectsQueryHandlerTest
    {
        private readonly Mock<IProjectRepository> _projectRepo = new();
        private readonly Mock<ITimesheetRepository> _timesheetRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IAuthService> _authService = new();

        private GetPagedProjectsQueryHandler CreateHandler()
            => new GetPagedProjectsQueryHandler(
                _projectRepo.Object,
                _timesheetRepo.Object,
                _mapper.Object,
                _authService.Object);

        private static Project CreateProject()
        {
            return new Project
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = "Projeto Teste"
            };
        }

        [Fact]
        public async Task Handle_Should_Return_PagedProjects_With_Time()
        {
            var project = CreateProject();
            var projects = new List<Project> { project };

            var userId = Guid.NewGuid();
            var time = TimeSpan.FromHours(5);

            _projectRepo
                .Setup(r => r.GetAllAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            _projectRepo
                .Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _authService
                .Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _timesheetRepo
                .Setup(t => t.GetTotalTimeByProjectsAndUserAsync(
                    It.IsAny<List<Guid>>(),
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<Guid, TimeSpan>
                {
                    { project.ProjectId, time }
                });

            _mapper
                .Setup(m => m.Map<ProjectResponseDto>(project))
                .Returns(new ProjectResponseDto
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName
                });

            var handler = CreateHandler();

            var result = await handler.Handle(
                new GetPagedProjectsQuery
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().HaveCount(1);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);

            result.Items.First().StatusTime.Should().NotBeNull();

            _projectRepo.Verify(r => r.GetAllAsync(1, 10, It.IsAny<CancellationToken>()), Times.Once);
            _projectRepo.Verify(r => r.CountAsync(It.IsAny<CancellationToken>()), Times.Once);
            _authService.Verify(a => a.GetCurrentUserAsync(), Times.Once);
            _timesheetRepo.Verify(t =>
                t.GetTotalTimeByProjectsAndUserAsync(
                    It.IsAny<List<Guid>>(),
                    userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Set_ZeroTime_When_No_Time_Found()
        {
            var project = CreateProject();
            var projects = new List<Project> { project };

            var userId = Guid.NewGuid();

            _projectRepo
                .Setup(r => r.GetAllAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            _projectRepo
                .Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _authService
                .Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _timesheetRepo
                .Setup(t => t.GetTotalTimeByProjectsAndUserAsync(
                    It.IsAny<List<Guid>>(),
                    userId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<Guid, TimeSpan>());

            _mapper
                .Setup(m => m.Map<ProjectResponseDto>(project))
                .Returns(new ProjectResponseDto
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName
                });

            var handler = CreateHandler();

            var result = await handler.Handle(
                new GetPagedProjectsQuery
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                CancellationToken.None);

            result.Items.First().StatusTime.Should().NotBeNull();
        }
    }
}
