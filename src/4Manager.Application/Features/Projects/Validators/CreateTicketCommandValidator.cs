using _4Tech._4Manager.Application.Features.Projects.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketCommandValidator() 
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O ProjectId inserido não é um Guid válido.");

            RuleFor(x => x)
                .Must(x => x.OpeningDate <= x.DeadlineDate)
                .WithMessage("A data de início não pode ser maior que a data final.");
        }
    }
}
