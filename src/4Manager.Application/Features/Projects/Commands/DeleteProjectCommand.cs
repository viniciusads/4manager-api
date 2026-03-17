using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class DeleteProjectCommand : IRequest<ProjectResponseDto>
    {
        public Guid ProjectId { get; set; }
        public DeleteProjectCommand(Guid projectId)
        {
            ProjectId = projectId;
        }
    }

}
