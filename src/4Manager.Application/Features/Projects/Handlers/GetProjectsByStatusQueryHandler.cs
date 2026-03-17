using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Common.Date;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetProjectsByStatusQueryHandler : IRequestHandler<GetProjectsByStatusQuery, IEnumerable<ProjectResponseDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ITimesheetRepository _timesheetRepository;

        public GetProjectsByStatusQueryHandler(IProjectRepository projectRepository, IAuthService authService, ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _timesheetRepository = timesheetRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<ProjectResponseDto>> Handle(GetProjectsByStatusQuery request, CancellationToken cancellationToken)
        {
            var filteredProjects = await _projectRepository.GetByStatusAsync(
                request.statusProject,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            var currentUser = await _authService.GetCurrentUserAsync();

            var result = new List<ProjectResponseDto>();

            foreach (var project in filteredProjects)
            {
                var totalTime = await _timesheetRepository
                    .GetTotalTimeByProjectAndUserAsync(
                        project.ProjectId,
                        currentUser,
                        cancellationToken
                    );

                var dto = _mapper.Map<ProjectResponseDto>(project);
                dto.StatusTime = totalTime.FormatTime();

                result.Add(dto);
            }

            if (filteredProjects == null || !filteredProjects.Any())
                throw new GuidFoundException("Não existe nenhum projeto com esse status no momento.");

            return result;
        }
    }
}
