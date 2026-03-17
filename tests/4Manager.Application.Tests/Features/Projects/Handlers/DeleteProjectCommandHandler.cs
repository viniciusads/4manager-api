using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public DeleteProjectCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Project, ProjectResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Delete_Project_And_Return_Dto()
        {
            var projectId = Guid.NewGuid(); 
            var project = new Project { ProjectId = projectId };

            var mockRepo = new Mock<IProjectRepository>();
            mockRepo
                .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            var command = new DeleteProjectCommand(projectId);
            var handler = new DeleteProjectCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.ProjectId);

            mockRepo.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.DeleteProjectAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_ProjectNotFoundException_When_Project_Does_Not_Exist()
        {
            var projectId = Guid.NewGuid();
            var mockRepo = new Mock<IProjectRepository>();

            mockRepo
                .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project?)null);

            var handler = new DeleteProjectCommandHandler(mockRepo.Object, _mapper);
            var command = new DeleteProjectCommand(projectId);

            await Assert.ThrowsAsync<ProjectException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(r => r.DeleteProjectAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
