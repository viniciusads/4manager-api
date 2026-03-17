using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Timesheets.Validators
{
    public class GetTimesheetByIdQueryValidator : AbstractValidator<GetTimesheetByIdQuery>
    {
        public GetTimesheetByIdQueryValidator()
        {
            RuleFor(x => x.TimesheetId)
                .NotEmpty()
                .WithMessage("O id informado não é um id válido!");
        }
    }
}
