using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetProjectsByStatusQueryHandler : IRequestHandler<GetProjectsByStatusQuery, IEnumerable<ProjectResponseDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<IEnumerable<ProjectResponseDto>> _validator;


        public GetProjectsByStatusQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponseDto>> Handle(GetProjectsByStatusQuery request, CancellationToken cancellationToken)
        {
            var filteredProjects = await _projectRepository.GetByStatusAsync(
                request.statusProject,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            if (filteredProjects == null || !filteredProjects.Any())
                throw new NotFoundException("Não existe nenhum projeto com esse status no momento.");

            return _mapper.Map<IEnumerable<ProjectResponseDto>>(filteredProjects);
        }
    }
}
