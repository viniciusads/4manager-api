using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class GetProjectsByStatusQueryHandlerTests
    {
        private readonly ProjectTestFixture _fixture;

        public GetProjectsByStatusQueryHandlerTests()
        {
            _fixture = new ProjectTestFixture();
        }

        [Fact]
        public async Task ReturnProjectStatusList()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockTRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockClient = new Mock<IAuthService>();
            var cancellationToken = CancellationToken.None;

            var status = ProjectStatusEnum.Concluido;

            var projectEntities = _fixture.GenerateTestProjects(2).ToList();
            projectEntities[0].StatusProject = ProjectStatusEnum.Concluido;
            projectEntities[1].StatusProject = ProjectStatusEnum.Concluido;

            var projectResponseDtos = projectEntities.Select(p => new ProjectResponseDto
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                StatusProject = p.StatusProject
            }).ToList();

            mockRepository
                .Setup(repo => repo.GetByStatusAsync(status, It.IsAny<int>(), It.IsAny<int>(), cancellationToken))
                .ReturnsAsync(projectEntities);

            mockClient
                .Setup(c => c.GetCurrentUserAsync());

          
            mockTRepository
                .Setup(t => t.GetTotalTimeByProjectAndUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), cancellationToken))
                .ReturnsAsync(TimeSpan.FromHours(1));

            
            mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()))
                .Returns((Project p) => new ProjectResponseDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    StatusProject = p.StatusProject,
                    StatusTime = "01:00:00"
                });

            var handler = new GetProjectsByStatusQueryHandler(
                mockRepository.Object,
                mockClient.Object,
                mockTRepository.Object,
                mockMapper.Object
            );

            var query = new GetProjectsByStatusQuery(status);

            var result = await handler.Handle(query, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(status, p.StatusProject));
        }


        [Fact]
        public async Task NoProjectsWithRequiredStatusFound()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();
            var cancellationToken = CancellationToken.None;
            var mockClient = new Mock<IAuthService>(); 
            var mockTRepository = new Mock<ITimesheetRepository>();

            var status = ProjectStatusEnum.Ativo;

            mockRepository
                .Setup(repo => repo.GetByStatusAsync(
                    status, It.IsAny<int>(), It.IsAny<int>(), cancellationToken))
                .ReturnsAsync(new List<Project>());

            var handler = new GetProjectsByStatusQueryHandler(
                mockRepository.Object,
                mockClient.Object,
                mockTRepository.Object,
                mockMapper.Object
            );

            await Assert.ThrowsAsync<GuidFoundException>(async () =>
                await handler.Handle(new GetProjectsByStatusQuery(status), cancellationToken)
            );
        }
    }
}
