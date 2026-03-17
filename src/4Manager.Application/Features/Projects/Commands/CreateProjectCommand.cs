using _4Tech._4Manager.Application.Features.Projects.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class CreateProjectCommand : IRequest<ProjectResponseDto>
    {

        public string ProjectName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string TitleColor { get; set; } = string.Empty;
        public Guid? CustomerManagerId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid ManagerId { get; set; }
        public List<Guid> CollaboratorIds { get; set; } = new List<Guid>();
    } 
}
