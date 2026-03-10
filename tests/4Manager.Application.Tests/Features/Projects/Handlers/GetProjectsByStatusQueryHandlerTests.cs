using _4tech._4Manager.Application.Features.Projects.Dtos;
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
            var mockMapper = new Mock<IMapper>();
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

            mockMapper
                .Setup(m => m.Map<IEnumerable<ProjectResponseDto>>(It.IsAny<IEnumerable<Project>>()))
                .Returns(projectResponseDtos);

            var handler = new GetProjectsByStatusQueryHandler(mockRepository.Object, mockMapper.Object);
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

            var status = ProjectStatusEnum.Ativo;

            mockRepository
                .Setup(repo => repo.GetByStatusAsync(
                    status, It.IsAny<int>(), It.IsAny<int>(), cancellationToken))
                .ReturnsAsync(new List<Project>());

            var handler = new GetProjectsByStatusQueryHandler(mockRepository.Object, mockMapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new GetProjectsByStatusQuery(status), cancellationToken)
            );
        }
    }
}
