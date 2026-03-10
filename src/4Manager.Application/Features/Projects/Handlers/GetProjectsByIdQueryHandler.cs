using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class GetProjectsByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponseDto?>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectsByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.projectId, cancellationToken);

            if (project is null)
                throw new NotFoundException("O Guid solicitado não se encontra no banco de dados.");

            return _mapper.Map<ProjectResponseDto>(project);
        }
    }
}
