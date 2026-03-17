using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services
{
    public class TimesheetReportCalculator : ITimesheetReportCalculator
    {
        public TimesheetReportDataGroupResponseDto Calculate(List<TimesheetReportResponseDto> timesheets)
        {
            var totalHours = timesheets.Sum(t => (t.EndDate.Value - t.StartDate).TotalHours);
            var totalTicks = timesheets.Sum(t => (t.EndDate.Value - t.StartDate).Ticks);
            TimeSpan time = TimeSpan.FromTicks(totalTicks);
            var formattedTime = $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";

            foreach (var t in timesheets)
            {
                var duration = t.EndDate!.Value - t.StartDate;
                t.Duration = duration;
                t.Hours =
                    $"{(int)duration.TotalHours:D2}:" +
                    $"{duration.Minutes:D2}:" +
                    $"{duration.Seconds:D2}";
                t.Time =
                    $"{t.StartDate:HH:mm} - {t.EndDate:HH:mm}<br/>" +
                    $"{t.StartDate:dd/MM/yyyy}";
            }

            var totalSeconds = timesheets.Sum(t => t.Duration.TotalSeconds);
            var totalTime = TimeSpan.FromSeconds(totalSeconds);

            return new TimesheetReportDataGroupResponseDto
            {
                TotalHours = formattedTime,
                Timesheets = timesheets,

                ActivityPercentages = timesheets
                    .GroupBy(t => new { t.ActivityTypeId, t.ActivityTypeName, t.ActivityColor })
                    .Select(a =>
                    {
                        var activitySeconds = a.Sum(x => x.Duration.TotalSeconds);
                        return new ActivityTypePercentageDto
                        {
                            ActivityTypeId = a.Key.ActivityTypeId,
                            ActivityTypeName = a.Key.ActivityTypeName,
                            ActivityColor = a.Key.ActivityColor,
                            Percentage = Math.Round((activitySeconds / totalSeconds) * 100, 2).ToString()
                        };
                    }).ToList(),

                ProjectPercentages = timesheets
                    .GroupBy(t => new { t.ProjectName, t.ProjectId, t.BlockColor })
                    .Select(a =>
                    {
                        var projectSeconds = a.Sum(x => x.Duration.TotalSeconds);

                        return new ProjectPercentageDto
                        {
                            ProjectName = a.Key.ProjectName,
                            ProjectId = a.Key.ProjectId,
                            BlockColor = a.Key.BlockColor,
                            Percentage = Math.Round((projectSeconds / totalSeconds) * 100, 2).ToString()
                        };
                    }).ToList()
            };
        }
    }
}
