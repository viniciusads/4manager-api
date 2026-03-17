using _4Tech._4Manager.Application.Common.Date;
using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectResponseDto>>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ITimesheetRepository _timesheetRepository;

        public GetProjectsQueryHandler(IProjectRepository repository, ITimesheetRepository timesheetRepository, IMapper mapper, IAuthService authService)
        {
            _repository = repository;
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
            _authService = authService;
           
        }
        public async Task<IEnumerable<ProjectResponseDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var pagedProjects = await _repository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            var currentUser = await _authService.GetCurrentUserAsync();

            var result = new List<ProjectResponseDto>();

            foreach (var project in pagedProjects)
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

            if (pagedProjects == null)
                throw new ProjectException("Não existem projetos no momento.");

            return result;

        }
    }
}
