using _4Tech._4Manager.Application.Features.Projects.Dtos;
using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Exceptions;
using AutoMapper;
using MediatR;
using _4Tech._4Manager.Application.Common.Exceptions;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectResponseDto>
    {
        public readonly IProjectRepository _repository;
        public readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
                throw new ProjectException();

            project.DeliveryDate = request.DeliveryDate;
            project.ProjectName = request.ProjectName;
            project.CustomerId = request.CustomerId;
            project.TitleColor = request.TitleColor;

            await _repository.UpdateProjectAsync(project, cancellationToken);

            return _mapper.Map<ProjectResponseDto>(project);
        }

    }
}
