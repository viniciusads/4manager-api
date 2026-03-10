using _4Tech._4Manager.Application.Features.Projects.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
    {
        public GetProjectByIdQueryValidator()
        {
            RuleFor(x => x.projectId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O id inserido não é um Guid válido.");
        }
    }
}
