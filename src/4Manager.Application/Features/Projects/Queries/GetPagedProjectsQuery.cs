using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public class GetPagedProjectsQuery : IRequest<PagedProjectResponseDto>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
