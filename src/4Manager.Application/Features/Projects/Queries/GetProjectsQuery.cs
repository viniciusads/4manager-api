using _4tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetProjectsQuery : IRequest<IEnumerable<ProjectResponseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
