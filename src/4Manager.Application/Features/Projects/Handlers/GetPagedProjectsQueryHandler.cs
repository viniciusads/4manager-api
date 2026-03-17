using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Common.Date;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetPagedProjectsQueryHandler : IRequestHandler<GetPagedProjectsQuery, PagedProjectResponseDto>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ITimesheetRepository _timesheetRepository;

        public GetPagedProjectsQueryHandler(
            IProjectRepository repository,
            ITimesheetRepository timesheetRepository,
            IMapper mapper,
            IAuthService authService)
        {
            _repository = repository;
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<PagedProjectResponseDto> Handle(GetPagedProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = (await _repository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            )).ToList();

            var totalCount = await _repository.CountAsync(cancellationToken);

            var projectIds = projects.Select(p => p.ProjectId).ToList();
            var currentUser = await _authService.GetCurrentUserAsync();

            var totals = await _timesheetRepository.GetTotalTimeByProjectsAndUserAsync(
                projectIds,
                currentUser,
                cancellationToken
            );

            var result = projects.Select(p =>
            {
                var dto = _mapper.Map<ProjectResponseDto>(p);
                dto.StatusTime = totals.TryGetValue(p.ProjectId, out var time)
                    ? time.FormatTime()
                    : TimeSpan.Zero.FormatTime();
                return dto;
            }).ToList();

            return new PagedProjectResponseDto
            {
                Items = result,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
