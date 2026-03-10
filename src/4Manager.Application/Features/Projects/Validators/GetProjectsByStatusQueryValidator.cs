using _4Tech._4Manager.Application.Features.Projects.Queries;
using FluentValidation;

namespace _4Tech._4Manager.Application.Features.Projects.Validators
{
    public class GetProjectsByStatusQueryValidator : AbstractValidator<GetProjectsByStatusQuery>
    {
        public GetProjectsByStatusQueryValidator()
        {
            RuleFor(x => x.statusProject)
                .IsInEnum()
                .WithMessage("O status informado é inválido. O status deve ser de 1 a 4");
        }
    }

}
