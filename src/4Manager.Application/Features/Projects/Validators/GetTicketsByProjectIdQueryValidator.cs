using _4Tech._4Manager.Application.Features.Projects.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class GetTicketsByProjectIdQueryValidator : AbstractValidator<GetTicketsByProjectIdQuery>
    {
        public GetTicketsByProjectIdQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O id inserido não é um Guid válido.");
        }
    }


}
