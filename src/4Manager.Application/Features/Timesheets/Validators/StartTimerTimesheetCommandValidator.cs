using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Timesheets.Validators
{
    public class StartTimerTimesheetCommandValidator : AbstractValidator<StartTimerTimesheetCommand>
    {
        public StartTimerTimesheetCommandValidator() {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("A data de início não pode ser nula");
        }
    }
}
