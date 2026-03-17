using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class GetProjectsQueryHandlerTests
    {
        private readonly ProjectTestFixture _fixture;

        public GetProjectsQueryHandlerTests()
        {
            _fixture = new ProjectTestFixture();
        }

        [Fact]
        public async Task ReturnListOfProjectResponseDto()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockTRepository = new Mock<ITimesheetRepository>();
            var mockClient = new Mock<IAuthService>();

            var projects = _fixture.GenerateTestProjects(2).ToList();

            mockRepository
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int page, int size, CancellationToken ct) =>
                    projects
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToList()
                );

            mockClient
                .Setup(c => c.GetCurrentUserAsync());

            mockTRepository
                .Setup(t => t.GetTotalTimeByProjectAndUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TimeSpan.FromHours(1));


            mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()))
                .Returns((Project p) => new ProjectResponseDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    StatusProject = p.StatusProject
                });


            var handler = new GetProjectsQueryHandler(mockRepository.Object, mockTRepository.Object, mockMapper.Object, mockClient.Object);

            var query = new GetProjectsQuery { PageNumber = 1, PageSize = 10 };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ReturnEmptyListWhenNoProjects()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockTRepository = new Mock<ITimesheetRepository>();
            var mockClient = new Mock<IAuthService>();

            mockClient
              .Setup(c => c.GetCurrentUserAsync());

            mockTRepository
                .Setup(t => t.GetTotalTimeByProjectAndUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny < CancellationToken>()))
                .ReturnsAsync(TimeSpan.FromHours(1));


            mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Project>());

            mockMapper
                .Setup(m => m.Map<IEnumerable<ProjectResponseDto>>(It.IsAny<IEnumerable<Project>>()))
                .Returns(new List<ProjectResponseDto>());

            var handler = new GetProjectsQueryHandler(mockRepository.Object, mockTRepository.Object, mockMapper.Object, mockClient.Object);

            var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
