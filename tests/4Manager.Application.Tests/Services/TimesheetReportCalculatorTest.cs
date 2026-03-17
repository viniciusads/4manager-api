using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.TimesheetReports.Services;
using FluentAssertions;

namespace _4Tech._4Manager.Application.Tests.Services
{
    public class TimesheetReportCalculatorTest
    {
        private readonly TimesheetReportCalculator _calculator;

        public TimesheetReportCalculatorTest()
        {
            _calculator = new TimesheetReportCalculator();
        }

        [Fact]
        public void Calculate_Should_Calculate_Total_And_Percentages_Correctly()
        {
            var start1 = new DateTime(2026, 01, 01, 8, 0, 0);
            var end1 = start1.AddHours(2); 

            var start2 = new DateTime(2026, 01, 01, 10, 0, 0);
            var end2 = start2.AddHours(2); 

            var timesheets = new List<TimesheetReportResponseDto>
        {
            new()
            {
                StartDate = start1,
                EndDate = end1,
                ActivityTypeId = Guid.NewGuid(),
                ActivityTypeName = "Dev",
                ActivityColor = "#111",
                ProjectId = Guid.NewGuid(),
                ProjectName = "Project A",
                BlockColor = "#AAA"
            },
            new()
            {
                StartDate = start2,
                EndDate = end2,
                ActivityTypeId = Guid.NewGuid(),
                ActivityTypeName = "Test",
                ActivityColor = "#222",
                ProjectId = Guid.NewGuid(),
                ProjectName = "Project B",
                BlockColor = "#BBB"
            }
        };

            var result = _calculator.Calculate(timesheets);

            result.TotalHours.Should().Be("04:00:00");

            result.Timesheets.Should().AllSatisfy(t =>
            {
                t.Duration.TotalHours.Should().Be(2);
                t.Hours.Should().Be("02:00:00");
                t.Time.Should().Contain(" - ");
            });

            result.ActivityPercentages.Should().HaveCount(2);
            result.ActivityPercentages.Should().AllSatisfy(a =>
                a.Percentage.Should().Be("50")
            );

            result.ProjectPercentages.Should().HaveCount(2);
            result.ProjectPercentages.Should().AllSatisfy(p =>
                p.Percentage.Should().Be("50")
            );
        }

        [Fact]
        public void Calculate_Should_Return_100_Percent_When_Single_Item()
        {
            var start = new DateTime(2026, 01, 01, 8, 0, 0);
            var end = start.AddHours(3);

            var timesheets = new List<TimesheetReportResponseDto>
        {
            new()
            {
                StartDate = start,
                EndDate = end,
                ActivityTypeId = Guid.NewGuid(),
                ActivityTypeName = "Dev",
                ActivityColor = "#111",
                ProjectId = Guid.NewGuid(),
                ProjectName = "Project A",
                BlockColor = "#AAA"
            }
        };

            var result = _calculator.Calculate(timesheets);

            result.TotalHours.Should().Be("03:00:00");
            result.ActivityPercentages.Single().Percentage.Should().Be("100");
            result.ProjectPercentages.Single().Percentage.Should().Be("100");
        }

        [Fact]
        public void Calculate_Should_Group_By_Same_Activity_And_Project()
        {
            var activityId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var start = new DateTime(2026, 01, 01, 8, 0, 0);

            var timesheets = new List<TimesheetReportResponseDto>
        {
            new()
            {
                StartDate = start,
                EndDate = start.AddHours(1),
                ActivityTypeId = activityId,
                ActivityTypeName = "Dev",
                ActivityColor = "#111",
                ProjectId = projectId,
                ProjectName = "Project A",
                BlockColor = "#AAA"
            },
            new()
            {
                StartDate = start.AddHours(1),
                EndDate = start.AddHours(3),
                ActivityTypeId = activityId,
                ActivityTypeName = "Dev",
                ActivityColor = "#111",
                ProjectId = projectId,
                ProjectName = "Project A",
                BlockColor = "#AAA"
            }
        };

            var result = _calculator.Calculate(timesheets);

            result.TotalHours.Should().Be("03:00:00");

            result.ActivityPercentages.Should().HaveCount(1);
            result.ProjectPercentages.Should().HaveCount(1);

            result.ActivityPercentages.Single().Percentage.Should().Be("100");
            result.ProjectPercentages.Single().Percentage.Should().Be("100");
        }

        [Fact]
        public void Calculate_Should_Handle_Empty_List()
        {
            var timesheets = new List<TimesheetReportResponseDto>();

            var result = _calculator.Calculate(timesheets);

            result.TotalHours.Should().Be("00:00:00");
            result.ActivityPercentages.Should().BeEmpty();
            result.ProjectPercentages.Should().BeEmpty();
        }
    }
}
