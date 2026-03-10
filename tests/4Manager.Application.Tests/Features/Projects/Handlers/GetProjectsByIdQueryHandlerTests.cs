using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class GetProjectsByIdQueryHandlerTests
    {
        private readonly ProjectTestFixture _fixture;

        public GetProjectsByIdQueryHandlerTests()
        {
            _fixture = new ProjectTestFixture();
        }

        [Fact]
        public async Task ReturnProjectWithRequiredId()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();
            var cancellationToken = CancellationToken.None;

            var projectEntity = _fixture.GenerateTestProjects(1).First();

            var projectResponseDto = new ProjectResponseDto
            {
                ProjectId = projectEntity.ProjectId,
                ProjectName = projectEntity.ProjectName,
                StatusProject = projectEntity.StatusProject
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(projectEntity.ProjectId, cancellationToken))
                .ReturnsAsync(projectEntity);

            mockMapper.Setup(m => m.Map<ProjectResponseDto>(projectEntity))
                .Returns(projectResponseDto);

            var handler = new GetProjectsByIdQueryHandler(mockRepository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetProjectByIdQuery(projectEntity.ProjectId), cancellationToken);

            Assert.NotNull(result);

            Assert.Equal(projectEntity.ProjectId, result.ProjectId);
            Assert.Equal(projectEntity.ProjectName, result.ProjectName);
            Assert.Equal(projectEntity.StatusProject, result.StatusProject);

            mockRepository.Verify(r => r.GetByIdAsync(projectEntity.ProjectId, cancellationToken), Times.Once());
            mockMapper.Verify(m => m.Map<ProjectResponseDto>(projectEntity), Times.Once());
        }

        [Fact]
        public async Task NoProjectsWithRequiredIdFound()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();
            var cancellationToken = CancellationToken.None;

            mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), cancellationToken))
                .ReturnsAsync((Project)null);

            var handler = new GetProjectsByIdQueryHandler(mockRepository.Object, mockMapper.Object);

            var query = new GetProjectByIdQuery(Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(query, cancellationToken)
            );

            mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Never());
        }
    }
}
