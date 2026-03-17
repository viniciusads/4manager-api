using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Features.Timesheets.Validators;
using FluentValidation.TestHelper;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Validators
{
    public class GetTimesheetByPeriodQueryValidatorTest
    {
        private readonly GetTimesheetByPeriodQueryValidator _validator;

        public GetTimesheetByPeriodQueryValidatorTest()
        {
            _validator = new GetTimesheetByPeriodQueryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Empty()
        {
            var query = new GetTimesheetsByPeriodQuery(default, DateTime.Today, Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.StartDate)
                  .WithErrorMessage("A data inicial é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Empty()
        {
            var query = new GetTimesheetsByPeriodQuery(DateTime.Today, default, Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.EndDate)
                  .WithErrorMessage("A data final é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Greater_Than_EndDate()
        {
            var query = new GetTimesheetsByPeriodQuery(DateTime.Today, DateTime.Today.AddDays(-1), Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("A data inicial não pode ser maior que a data final.");
        }

        [Fact]
        public void Should_Have_Error_When_Period_Is_Greater_Than_31_Days()
        {
            var query = new GetTimesheetsByPeriodQuery(DateTime.Today, DateTime.Today.AddDays(32), Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("O intervalo não pode ser maior que 31 dias.");
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Data_Is_Valid()
        {
            var query = new GetTimesheetsByPeriodQuery(DateTime.Today, DateTime.Today.AddDays(15), Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
