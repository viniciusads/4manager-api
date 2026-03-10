using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Domain.Enums;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Queries
{
    public class GetProjectsByStatusQuery : IRequest<IEnumerable<ProjectResponseDto>>
    {
        public ProjectStatusEnum statusProject { get; set;}
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetProjectsByStatusQuery(ProjectStatusEnum status)
        {
            statusProject = status;
        }
    }
}
