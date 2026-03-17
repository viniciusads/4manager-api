using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public class GetTicketsByProjectIdQuery : IRequest<IEnumerable<TicketResponseDto>>
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetTicketsByProjectIdQuery()
        {

        }

        public GetTicketsByProjectIdQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
