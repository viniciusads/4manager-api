using _4tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectResponseDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        public DeleteProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        public async Task<ProjectResponseDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                throw new ProjectNotFoundException($"Projeto com id {request.ProjectId} não encontrado.");

            await _projectRepository.DeleteProjectAsync(request.ProjectId, cancellationToken);

            return _mapper.Map<ProjectResponseDto>(project);
        }
    }
}
