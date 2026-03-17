using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Timesheets.Validators
{
    public class CreateManualTimesheetCommandValidator : AbstractValidator<CreateManualTimesheetCommand>
    {
        public CreateManualTimesheetCommandValidator()
        {

            RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("A data inicial é obrigatória.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("A data final é obrigatória.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.EndDate)
                .WithMessage("A data inicial não pode ser maior que a data final.");

            RuleFor(x => x.ActivityTypeId)
                .NotEmpty().WithMessage("É obrigatório inserir um tipo de atividade!");
        }
    }
}
