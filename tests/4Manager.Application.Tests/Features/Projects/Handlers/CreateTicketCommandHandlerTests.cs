using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using _4Tech._4Manager.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers { 
    public class CreateTicketCommandHandlerTests : IClassFixture<TicketTestFixture>
    {
        private readonly TicketTestFixture _fixture;
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly CreateTicketCommandHandler _handler;

        public CreateTicketCommandHandlerTests(TicketTestFixture fixture)
        {
            _fixture = fixture;
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();

            _handler = new CreateTicketCommandHandler(
                _ticketRepositoryMock.Object,
                _projectRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CreateTicketWhenValid()
        {
            var fakeTicket = _fixture.GenerateTestTickets(1)[0];
            var projectId = Guid.NewGuid();

            var command = new CreateTicketCommand
            {
                ProjectId = projectId,
                Applicant = fakeTicket.Applicant,
                Sector = fakeTicket.Sector,
                TicketResponsible = fakeTicket.TicketResponsible,
                Description = fakeTicket.Description,
                AffectedSystem = fakeTicket.AffectedSystem,
                ResponsibleArea = fakeTicket.ResponsibleArea,
                Priority = fakeTicket.Priority,
                TicketStatus = TicketStatusEnum.Aberto,
                OpeningDate = DateTime.UtcNow,
                DeadlineDate = DateTime.UtcNow.AddDays(2)
            };

            _ticketRepositoryMock
                .Setup(x => x.GetNextTicketNumberAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(15);

            _projectRepositoryMock
                .Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Project { ProjectId = projectId });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();

            _ticketRepositoryMock.Verify(
                x => x.CreateTicketAsync(It.Is<Ticket>(t =>
                    t.ProjectId == command.ProjectId &&
                    t.Description == command.Description &&
                    t.Priority == command.Priority &&
                    t.TicketNumber == 15 &&
                    t.TicketStatus == TicketStatusEnum.Aberto
                ), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task IfProjectNotFoundException()
        {
            var command = new CreateTicketCommand
            {
                ProjectId = Guid.NewGuid(),
                Applicant = "João",
                Sector = "TI",
                TicketResponsible = "Maria",
                Description = "Erro no sistema",
                AffectedSystem = "Financeiro",
                ResponsibleArea = "TI",
                Priority = TicketPriorityEnum.Media,
                OpeningDate = DateTime.UtcNow,
                DeadlineDate = DateTime.UtcNow.AddDays(3)
            };

            _projectRepositoryMock
                .Setup(x => x.GetByIdAsync(command.ProjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project?)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}