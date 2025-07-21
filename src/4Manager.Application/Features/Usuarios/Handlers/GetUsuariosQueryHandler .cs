using _4Manager.Application.Features.Usuarios.Dtos;
using _4Manager.Application.Features.Usuarios.Queries;
using _4Manager.Domain.Interfaces;
using MediatR;


namespace _4Manager.Application.Features.Usuarios.Handlers
{
    public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, IEnumerable<UsuarioDto>>
    {
        private readonly IUsuarioRepository _repository;

        public GetUsuariosQueryHandler(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _repository.GetAllAsync(cancellationToken);

            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            });
        }
    }
}
