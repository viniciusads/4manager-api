using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Validators;
using FluentValidation.TestHelper;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Validators
{
    public class CreateManualTimesheetCommandValidatorTest
    {
        private readonly CreateManualTimesheetCommandValidator _validator;

        public CreateManualTimesheetCommandValidatorTest()
        {
            _validator = new CreateManualTimesheetCommandValidator();
        }

        private CreateManualTimesheetCommand CreateValidCommand()
        {
            return new CreateManualTimesheetCommand
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Description = "Teste",
                ActivityTypeId = Guid.NewGuid()
            };
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            var command = CreateValidCommand();

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Empty()
        {
            var command = CreateValidCommand();
            command.StartDate = default;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Empty()
        {
            var command = CreateValidCommand();
            command.EndDate = default;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var command = CreateValidCommand();
            command.Description = "";

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Greater_Than_EndDate()
        {
            var command = CreateValidCommand();
            command.StartDate = DateTime.Now.AddHours(2);
            command.EndDate = DateTime.Now;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Have_Error_When_ActivityTypeId_Is_Empty()
        {
            var command = CreateValidCommand();
            command.ActivityTypeId = Guid.Empty;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ActivityTypeId);
        }
    }
}
