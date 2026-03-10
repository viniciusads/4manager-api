using _4Tech._4Manager.Application.Features.Team.Dtos;
using _4Tech._4Manager.Domain.Enums;

namespace _4tech._4Manager.Application.Features.Projects.Dtos
{
    public class ProjectResponseDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public Guid ClientId { get; set; }
        public ProjectStatusEnum StatusProject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string TitleColor { get; set; } = null!;
        public TeamResponseDto Team { get; set; } = null!;
        public DateTime StatusTime { get; set; }
        public bool Favorite { get; set; }
        public bool Archived { get; set; }
    }
}