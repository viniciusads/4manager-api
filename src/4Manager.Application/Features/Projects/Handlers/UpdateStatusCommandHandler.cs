using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;
using _4Tech._4Manager.Application.Common.Exceptions;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, ProjectResponseDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public UpdateStatusCommandHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDto> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
                throw new ProjectException();

            var updatedProject = await _projectRepository.UpdateStatusAsync(request.ProjectId, request.StatusProject, cancellationToken);
            return _mapper.Map<ProjectResponseDto>(updatedProject);
        }
    }
}
