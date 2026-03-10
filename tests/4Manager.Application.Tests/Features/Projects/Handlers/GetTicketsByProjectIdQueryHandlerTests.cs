using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Handlers;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Handlers
{
    public class GetTicketsByProjectIdQueryHandlerTests
    {
        private readonly TicketTestFixture _fixture;

        public GetTicketsByProjectIdQueryHandlerTests()
        {
            _fixture = new TicketTestFixture();
        }

        [Fact]
        public async Task ReturnListOfTicketsIfTheyExist()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var mockMapper = new Mock<IMapper>();

            var projectId = Guid.NewGuid();
            var tickets = _fixture.GenerateTestTickets(2);

            foreach (var t in tickets)
                t.ProjectId = projectId;

            mockRepository
                .Setup(repo => repo.GetByProjectIdAsync(
                    projectId,
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid p, int page, int size, CancellationToken ct) =>
                    tickets
                        .Where(t => t.ProjectId == p)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToList()
                );

            mockMapper
                .Setup(m => m.Map<IEnumerable<TicketResponseDto>>(It.IsAny<IEnumerable<Ticket>>()))
                .Returns<IEnumerable<Ticket>>(t =>
                    t.Select(x => new TicketResponseDto() { TicketId = x.TicketId })
                );

            var handler = new GetTicketsByProjectIdQueryHandler(mockRepository.Object, mockMapper.Object);

            var query = new GetTicketsByProjectIdQuery
            {
                ProjectId = projectId,
                PageNumber = 1,
                PageSize = 10
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
