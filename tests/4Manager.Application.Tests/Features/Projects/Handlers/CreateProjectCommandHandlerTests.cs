using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using _4Tech._4Manager.Domain.Exceptions;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class CreateProjectCommandHandlerTests : IClassFixture<ProjectTestFixture>
    {
        private readonly ProjectTestFixture _fixture;
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests(ProjectTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<IProjectRepository>();
            _handler = new CreateProjectCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateProjectWhenValid_WithCustomerManager_ShouldReturnProjectId()
        {
            var fakeProject = _fixture.GenerateTestProjects(1)[0];
            var managerId = Guid.NewGuid();
            var customerManagerId = Guid.NewGuid();
            var collaboratorIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var command = new CreateProjectCommand
            {
                ProjectName = fakeProject.ProjectName,
                CustomerId = fakeProject.CustomerId,
                CustomerManagerId = customerManagerId,
                StartDate = fakeProject.StartDate,
                DeliveryDate = fakeProject.DeliveryDate,
                TitleColor = fakeProject.TitleColor,
                ManagerId = managerId,
                CollaboratorIds = collaboratorIds,
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(customerManagerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Cliente);

            _repositoryMock
                .Setup(x => x.CollaboratorsExistAsync(collaboratorIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project p, CancellationToken ct) =>
                {
                    if (p.ProjectId == Guid.Empty)
                        p.ProjectId = Guid.NewGuid();
                    return p;
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();
            _repositoryMock.Verify(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.GetManagerRoleAsync(customerManagerId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.CollaboratorsExistAsync(collaboratorIds, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateProjectWhenValid_WithoutCustomerManager_ShouldReturnProjectId()
        {
            var fakeProject = _fixture.GenerateTestProjects(1)[0];
            var managerId = Guid.NewGuid();
            var collaboratorIds = new List<Guid> { Guid.NewGuid() };

            var command = new CreateProjectCommand
            {
                ProjectName = fakeProject.ProjectName,
                CustomerId = fakeProject.CustomerId,
                CustomerManagerId = null,
                StartDate = fakeProject.StartDate,
                DeliveryDate = fakeProject.DeliveryDate,
                TitleColor = fakeProject.TitleColor,
                ManagerId = managerId,
                CollaboratorIds = collaboratorIds,
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.CollaboratorsExistAsync(collaboratorIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project p, CancellationToken ct) =>
                {
                    if (p.ProjectId == Guid.Empty)
                        p.ProjectId = Guid.NewGuid();
                    return p;
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();
            _repositoryMock.Verify(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.GetManagerRoleAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once); 
        }

        [Fact]
        public async Task CreateProjectWhenManagerNotFound_ShouldThrowNotFoundException()
        {
            var command = new CreateProjectCommand
            {
                ProjectName = "Test Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = null,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = Guid.NewGuid(),
                CollaboratorIds = new List<Guid>(),
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(command.ManagerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoleEnum?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("O gerente 4Tech não foi encontrado.");
        }

        [Fact]
        public async Task CreateProjectWhenManagerHasWrongRole_ShouldThrowValidationException()
        {
            var command = new CreateProjectCommand
            {
                ProjectName = "Test Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = null,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = Guid.NewGuid(),
                CollaboratorIds = new List<Guid>(),
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(command.ManagerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Cliente); 

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("O gerente 4Tech não possui a role correta.");
        }

        [Fact]
        public async Task CreateProjectWhenCustomerManagerNotFound_ShouldThrowNotFoundException()
        {
            var managerId = Guid.NewGuid();
            var customerManagerId = Guid.NewGuid();

            var command = new CreateProjectCommand
            {
                ProjectName = "Test Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = customerManagerId,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = managerId,
                CollaboratorIds = new List<Guid>(),
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(customerManagerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoleEnum?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("O gestor do cliente não foi encontrado.");
        }

        [Fact]
        public async Task CreateProjectWhenCustomerManagerHasWrongRole_ShouldThrowValidationException()
        {
            var managerId = Guid.NewGuid();
            var customerManagerId = Guid.NewGuid();

            var command = new CreateProjectCommand
            {
                ProjectName = "Test Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = customerManagerId,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = managerId,
                CollaboratorIds = new List<Guid>(),
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(customerManagerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor); 

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("O gestor do cliente não possui a role correta.");
        }

        [Fact]
        public async Task CreateProjectWhenProjectNameExists_ShouldThrowValidationException()
        {
            var command = new CreateProjectCommand
            {
                ProjectName = "Existing Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = null,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = Guid.NewGuid(),
                CollaboratorIds = new List<Guid>(),
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Já existe um projeto com esse nome.");
        }

        [Fact]
        public async Task CreateProjectWhenCollaboratorsDoNotExist_ShouldThrowNotFoundException()
        {
            var managerId = Guid.NewGuid();
            var collaboratorIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var command = new CreateProjectCommand
            {
                ProjectName = "Test Project",
                CustomerId = Guid.NewGuid(),
                CustomerManagerId = null,
                StartDate = DateTime.UtcNow,
                DeliveryDate = DateTime.UtcNow.AddDays(30),
                TitleColor = "#FFFFFF",
                ManagerId = managerId,
                CollaboratorIds = collaboratorIds,
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.CollaboratorsExistAsync(collaboratorIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Um ou mais collaboratorIds não existem.");
        }

        [Fact]
        public async Task CreateProjectWhenValid_WithEmptyCollaborators_ShouldReturnProjectId()
        {
            var fakeProject = _fixture.GenerateTestProjects(1)[0];
            var managerId = Guid.NewGuid();

            var command = new CreateProjectCommand
            {
                ProjectName = fakeProject.ProjectName,
                CustomerId = fakeProject.CustomerId,
                CustomerManagerId = null,
                StartDate = fakeProject.StartDate,
                DeliveryDate = fakeProject.DeliveryDate,
                TitleColor = fakeProject.TitleColor,
                ManagerId = managerId,
                CollaboratorIds = new List<Guid>(), 
            };

            _repositoryMock
                .Setup(x => x.ProjectNameExistsAsync(command.ProjectName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(x => x.GetManagerRoleAsync(managerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(RoleEnum.Gestor);

            _repositoryMock
                .Setup(x => x.CollaboratorsExistAsync(new List<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); 

            _repositoryMock
                .Setup(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project p, CancellationToken ct) =>
                {
                    if (p.ProjectId == Guid.Empty)
                        p.ProjectId = Guid.NewGuid();
                    return p;
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();
            _repositoryMock.Verify(x => x.CreateProjectAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.CollaboratorsExistAsync(new List<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}