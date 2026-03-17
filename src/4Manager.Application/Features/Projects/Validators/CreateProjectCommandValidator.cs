using _4Tech._4Manager.Application.Features.Projects.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectName)
                .NotEmpty()
                .WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(200)
                .WithMessage("O nome do projeto não pode ter mais de 200 caracteres.");

            RuleFor(x => x.ManagerId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O ManagerId é obrigatório e deve ser um Guid válido.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.DeliveryDate)
                .WithMessage("A data de início não pode ser maior que a data de entrega.");

            RuleFor(x => x.TitleColor)
                .NotEmpty()
                .WithMessage("A cor do título é obrigatória.");
        }
    }
}
