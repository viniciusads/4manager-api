using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class UpdateProjectCommandHandlerTests
    {

        private readonly IMapper _mapper;
        private readonly ProjectTestFixture _fixture;

        public UpdateProjectCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Project, ProjectResponseDto>();
            });
            _mapper = config.CreateMapper();

            _fixture = new ProjectTestFixture();
        }

        [Fact]
        public async Task UpdateProject()
        {
            var projectId = Guid.NewGuid();
            var deliveryDate = new DateTime(2026, 01, 01);
            var customerId = Guid.NewGuid();
            var collaboratorsId = new List<Guid>();

            var fakeProject = _fixture.GenerateTestProjects(1)[0];
            fakeProject.ProjectId = projectId;
            fakeProject.CustomerId = customerId;
            fakeProject.ProjectName = "Projeto teste";
            fakeProject.TitleColor = "#FFFFF";

            var mockRepo = new Mock<IProjectRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProject);

            mockRepo.Setup(r => r.UpdateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .Returns((Project p, CancellationToken ct) => Task.FromResult(p));

            var command = new UpdateProjectCommand(fakeProject.ProjectId, fakeProject.CustomerId, fakeProject.ProjectName, fakeProject.TitleColor, collaboratorsId, deliveryDate);
            var handler = new UpdateProjectCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.ProjectId);

            mockRepo.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFailIfProjectNotFound()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();

            var collaboratorsId = new List<Guid>();
            var projectId = Guid.NewGuid();
            var deliveryDate = DateTime.UtcNow;

            var fakeProject = _fixture.GenerateTestProjects(1)[0];

            mockRepository.Setup(repo => repo.GetByIdAsync(fakeProject.ProjectId, CancellationToken.None))
                .ThrowsAsync(new GuidFoundException($"Project com id {fakeProject.ProjectId} não encontrado."));

            var query = new UpdateProjectCommand(fakeProject.ProjectId, fakeProject.CustomerId, fakeProject.ProjectName, fakeProject.TitleColor, collaboratorsId, deliveryDate);
            var handler = new UpdateProjectCommandHandler(mockRepository.Object, _mapper);

            var exceptionMessage = $"Project com id {fakeProject.ProjectId} não encontrado.";
            var exception = await Assert.ThrowsAsync<GuidFoundException>(() =>
                handler.Handle(query, CancellationToken.None));

            Assert.Equal(exceptionMessage, exception.Message);

            mockRepository.Verify(r => r.UpdateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowProjectException_WhenProjectNotFound()
        {
            var mockRepo = new Mock<IProjectRepository>();
            var mockMapper = new Mock<IMapper>();

            var projectId = Guid.NewGuid();

            mockRepo
                .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project?)null);

            var handler = new UpdateProjectCommandHandler(
                mockRepo.Object,
                mockMapper.Object);

            var command = new UpdateProjectCommand(
                projectId,
                Guid.NewGuid(),
                "Nome",
                "#FFFFFF",
                new List<Guid>(),
                DateTime.UtcNow);

            await Assert.ThrowsAsync<ProjectException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(r =>
                r.UpdateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
