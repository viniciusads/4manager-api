namespace _4Tech._4Manager.Application.Features.Projects.Dtos
{
    public class PagedProjectResponseDto
    {
        public IEnumerable<ProjectResponseDto> Items { get; set; } = new List<ProjectResponseDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage => PageNumber * PageSize < TotalCount;
        public bool HasPreviousPage => PageNumber > 1;
    }
}
