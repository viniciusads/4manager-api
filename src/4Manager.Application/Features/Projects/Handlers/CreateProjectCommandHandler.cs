using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using _4Tech._4Manager.Domain.Exceptions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _4Tech._4Manager.Application.Features.Projects.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectRepository _repository;

        public CreateProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateBusinessRulesAsync(request, cancellationToken);

            var newProject = CreateProject(request);

            var newTeam = CreateTeam(request, newProject);

            newProject.Team = newTeam;

            PopulateTeamCollaborators(newTeam, request);

            await _repository.CreateProjectAsync(newProject, cancellationToken);

            return newProject.ProjectId;
        }

        private async Task ValidateBusinessRulesAsync(
            CreateProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            if (await _repository.ProjectNameExistsAsync(request.ProjectName, cancellationToken))
                throw new ValidationException("Já existe um projeto com esse nome.");

            await ValidateManagerAsync(request.ManagerId, RoleEnum.Gestor, "O gerente 4Tech", cancellationToken);

            if (request.CustomerManagerId.HasValue)
                await ValidateManagerAsync(request.CustomerManagerId.Value, RoleEnum.Cliente, "O gestor do cliente", cancellationToken);

            if (!await _repository.CollaboratorsExistAsync(request.CollaboratorIds, cancellationToken))
                throw new NotFoundException("Um ou mais collaboratorIds não existem.");
        }

        private async Task ValidateManagerAsync(
            Guid managerId, 
            RoleEnum expectedRole, 
            string managerDescription,
            CancellationToken cancellationToken)
        {
            var role = await _repository.GetManagerRoleAsync(managerId, cancellationToken);

            if (!role.HasValue)
                throw new NotFoundException($"{managerDescription} não foi encontrado.");

            if (role.Value != expectedRole)
                throw new ValidationException($"{managerDescription} não possui a role correta.");
        }

        private Project CreateProject(CreateProjectCommand request)
        {
            return new Project
            {
                ProjectName = request.ProjectName,

                CustomerManagerId = request.CustomerManagerId,
                CustomerId = request.CustomerId,

                StartDate = request.StartDate.ToUniversalTime(),
                DeliveryDate = request.DeliveryDate.ToUniversalTime(),
                TitleColor = request.TitleColor,

                StatusProject = ProjectStatusEnum.Ativo,
                StatusTime = DateTime.UtcNow,
                Favorite = false,
                Archived = false
            };
        }

        private Domain.Entities.Team CreateTeam(CreateProjectCommand request, Project newProject)
        {
            return new Domain.Entities.Team
            {
                ProjectId = newProject.ProjectId,
                Project = newProject,
                ManagerId = request.ManagerId
            };
        }

        private void PopulateTeamCollaborators(Domain.Entities.Team team, CreateProjectCommand request)
        {
            foreach (var collabId in request.CollaboratorIds ?? Enumerable.Empty<Guid>())
            {
                team.Collaborators.Add(new TeamCollaborator
                {
                    Team = team,
                    CollaboratorId = collabId
                });
            }
        }
    }
}
