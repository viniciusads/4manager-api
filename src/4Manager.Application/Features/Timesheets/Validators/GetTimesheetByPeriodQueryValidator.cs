using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Timesheets.Validators
{
    public class GetTimesheetByPeriodQueryValidator : AbstractValidator<GetTimesheetsByPeriodQuery>
    {
        public GetTimesheetByPeriodQueryValidator()
        {
            RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("A data inicial é obrigatória.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("A data final é obrigatória.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.EndDate)
                .WithMessage("A data inicial não pode ser maior que a data final.");

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays <= 31)
                .WithMessage("O intervalo não pode ser maior que 31 dias.");
        }
    }
}
