using _4Tech._4Manager.Application.Features.Projects.Commands;
using _4Tech._4Manager.Application.Features.Projects.Validators;
using FluentValidation.TestHelper;

namespace _4Tech._4Manager.Application.Tests.Features.Projects.Validators
{
    public class CreateProjectCommandValidatorTest
    {
        private readonly CreateProjectCommandValidator _validator = new();

        private CreateProjectCommand CreateValidCommand()
        {
            return new CreateProjectCommand
            {
                ProjectName = "Projeto Teste",
                ManagerId = Guid.NewGuid(),
                StartDate = DateTime.Today,
                DeliveryDate = DateTime.Today.AddDays(10),
                TitleColor = "#FFFFFF"
            };
        }

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = CreateValidCommand();

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_ProjectName_Is_Empty()
        {
            var command = CreateValidCommand();
            command.ProjectName = string.Empty;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ProjectName);
        }

        [Fact]
        public void Should_Fail_When_ProjectName_Exceeds_Max_Length()
        {
            var command = CreateValidCommand();
            command.ProjectName = new string('A', 201);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ProjectName);
        }

        [Fact]
        public void Should_Fail_When_ManagerId_Is_Empty_Guid()
        {
            var command = CreateValidCommand();
            command.ManagerId = Guid.Empty;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ManagerId);
        }

        [Fact]
        public void Should_Fail_When_StartDate_Is_Greater_Than_DeliveryDate()
        {
            var command = CreateValidCommand();
            command.StartDate = DateTime.Today.AddDays(5);
            command.DeliveryDate = DateTime.Today;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Fail_When_TitleColor_Is_Empty()
        {
            var command = CreateValidCommand();
            command.TitleColor = string.Empty;

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TitleColor);
        }
    }
}
