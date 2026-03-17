namespace _4Tech._4Manager.Application.Features.ActivityTypes.Dtos
{
    public class ActivityTypeResponseDto
    {
        public Guid ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
        public string ActivityTypeColor {  get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}