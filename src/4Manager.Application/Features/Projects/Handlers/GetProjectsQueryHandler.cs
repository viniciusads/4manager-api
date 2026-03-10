using _4tech._4Manager.Application.Features.Projects.Dtos;
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

        public GetProjectsQueryHandler(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProjectResponseDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var pagedProjects = await _repository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );


            if (pagedProjects == null)
                throw new NotFoundException("Não existem projetos no momento.");

            return _mapper.Map<IEnumerable<ProjectResponseDto>>(pagedProjects);

        }
    }
}
