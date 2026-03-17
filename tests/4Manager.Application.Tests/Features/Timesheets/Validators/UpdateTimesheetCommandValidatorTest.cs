using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Validators;
using FluentValidation.TestHelper;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Validators
{
    public class UpdateTimesheetCommandValidatorTest
    {
        private readonly UpdateTimesheetCommandValidator _validator;

        public UpdateTimesheetCommandValidatorTest()
        {
            _validator = new UpdateTimesheetCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Empty()
        {
            var command = new UpdateTimesheetCommand
            {
                StartDate = default,
                EndDate = DateTime.Today,
                ActivityTypeId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.StartDate)
                  .WithErrorMessage("A data inicial é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Empty()
        {
            var command = new UpdateTimesheetCommand
            {
                StartDate = DateTime.Today,
                EndDate = default,
                ActivityTypeId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.EndDate)
                  .WithErrorMessage("A data final é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Greater_Than_EndDate()
        {
            var command = new UpdateTimesheetCommand
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(-1),
                ActivityTypeId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("A data inicial não pode ser maior que a data final.");
        }

        [Fact]
        public void Should_Have_Error_When_ActivityTypeId_Is_Empty()
        {
            var command = new UpdateTimesheetCommand
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ActivityTypeId = Guid.Empty
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ActivityTypeId)
                  .WithErrorMessage("É obrigatório inserir um tipo de atividade!");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            var command = new UpdateTimesheetCommand
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ActivityTypeId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
