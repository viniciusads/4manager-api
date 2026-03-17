using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class UpdateStatusHandlerTests
    {

        private readonly IMapper _mapper;
        private readonly ProjectTestFixture _fixture;

        public UpdateStatusHandlerTests() {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Project, ProjectResponseDto>();
            });
            _mapper = config.CreateMapper();

            _fixture = new ProjectTestFixture();
        }

        [Fact]
        public async Task UpdateStatus()
        {
            var projectId = Guid.NewGuid();
            var statusProject = ProjectStatusEnum.Pendente;

            var fakeProject = _fixture.GenerateTestProjects(1)[0];
            fakeProject.ProjectId = projectId;
            fakeProject.StatusProject = statusProject;

            var mockRepo = new Mock<IProjectRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProject);

            mockRepo.Setup(r => r.UpdateStatusAsync( projectId, statusProject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Project { ProjectId = projectId, ProjectName = "Updated Project", StatusProject = ProjectStatusEnum.Pendente });

            var command = new UpdateStatusCommand(projectId, statusProject );
            var handler = new UpdateStatusCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.ProjectId);
            Assert.Equal(ProjectStatusEnum.Pendente, result.StatusProject);

            mockRepo.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateStatusAsync(projectId, statusProject, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFailIfProjectNotFound()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();

            var statusProject = ProjectStatusEnum.Pendente;
            var projectId = Guid.NewGuid();

            var fakeProject = _fixture.GenerateTestProjects(1)[0];

            mockRepository.Setup(repo => repo.GetByIdAsync(fakeProject.ProjectId, CancellationToken.None))
                .ThrowsAsync(new GuidFoundException($"Project com id {fakeProject.ProjectId} não encontrado."));

            var query = new UpdateStatusCommand(fakeProject.ProjectId, statusProject);
            var handler = new UpdateStatusCommandHandler(mockRepository.Object, _mapper);

            var exceptionMessage = $"Project com id {fakeProject.ProjectId} não encontrado.";
            var exception = await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);

            mockRepository.Verify(r => r.UpdateStatusAsync(projectId, statusProject, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowProjectException_WhenProjectNotFound()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();

            var projectId = Guid.NewGuid();
            var status = ProjectStatusEnum.Pendente;

            mockRepository
                .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project?)null);

            var handler = new UpdateStatusCommandHandler(
                mockRepository.Object,
                mockMapper.Object);

            var command = new UpdateStatusCommand(projectId, status);

            await Assert.ThrowsAsync<ProjectException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockRepository.Verify(r =>
                r.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<ProjectStatusEnum>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
