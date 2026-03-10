using _4Tech._4Manager.Application.Features.Projects.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class GetTicketDetailsByTicketIdQueryValidator : AbstractValidator<GetTicketDetailsByTicketIdQuery>
    {
        public GetTicketDetailsByTicketIdQueryValidator()
        {
            RuleFor(x => x.ticketId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O id inserido não é um Guid válido.");
        }
    }
}
