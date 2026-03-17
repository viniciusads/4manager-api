using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using _4Tech._4Manager.Application.Features.TimesheetReports.Helpers;
using _4Tech._4Manager.Application.Features.TimesheetReports.Queries;
using _4Tech._4Manager.Application.Features.TimesheetReports.Services.Formatters;
using _4Tech._4Manager.Application.Interfaces;
using MediatR;
using System.Text;
using System.Text.Json;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Handlers
{
    public class ExportTimesheetReportQueryHandler
        : IRequestHandler<ExportTimesheetReportQuery, ExportFileResultDto>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPdfGeneratorService _pdfGenerator;

        public ExportTimesheetReportQueryHandler(ITimesheetRepository timesheetRepository, IUserRepository userRepository, IPdfGeneratorService pdfGenerator)
        {
            _timesheetRepository = timesheetRepository;
            _userRepository = userRepository;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<ExportFileResultDto> Handle(
            ExportTimesheetReportQuery request,
            CancellationToken cancellationToken)
        {
            var timesheets = await _timesheetRepository.GetByDateRangeAsync(
                request.StartDate,
                request.EndDate,
                request.AuthenticatedUserId,
                cancellationToken
            );

            var userName = await _userRepository.GetUserNameByIdAsync(
                request.AuthenticatedUserId,
                cancellationToken
            );

            var exportData = timesheets.Select(t => new TimesheetExportInternalDto
            {
                Description = t.Description,
                Duration = CalculateDuration(t.StartDate, t.EndDate),
                Member = userName,
                Project = t.Project?.ProjectName ?? "Sem Projeto",
                Customer = t.Customer?.Name ?? "-",
                ActivityType = t.ActivityType?.ActivityTypeName ?? "Sem atividade",
                // Tags = t.TagId?.ToString() ?? "", // TODO: Buscar tags quando implementadas
                StartDate = t.StartDate,
                StartTime = t.StartDate.ToString("HH:mm"),
                StopDate = t.EndDate ?? t.StartDate,
                StopTime = t.EndDate?.ToString("HH:mm") ?? "",
                ProjectId = t.Project?.ProjectId.ToString(),
                BlockColor = GetBlockColor(t)
            }).ToList();

            if (request.Format.ToLower() == "pdf")
            {
                var totalHours = CalculateTotalHours(exportData);
                var averageDailyHours = CalculateAverageDailyHours(exportData);

                var barChartData = PrepareBarChartData(exportData);
                var donutChartData = PrepareDonutChartData(exportData);
                var projectLegend = GenerateProjectLegend(donutChartData);

                var html = RenderHtmlTemplate(
                    "index.html",
                    userName,
                    totalHours,
                    averageDailyHours,
                    barChartData,
                    donutChartData,
                    projectLegend,
                    exportData
                );

                File.WriteAllText("DEBUG.html", html);

                var css = File.ReadAllText(Path.Combine(
                    AppContext.BaseDirectory,
                    "Features",
                    "TimesheetReports",
                    "Templates",
                    "style.css"
                ));

                var pdfBytes = await _pdfGenerator.GenerateFromHtmlAsync(html, css, cancellationToken);

                return new ExportFileResultDto(
                    pdfBytes,
                    $"relatorio_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.pdf",
                    "application/pdf"
                );
            }

            var formatter = ExportFormatterFactory.CreateFormatter(request.Format);

            var fileContent = formatter.Format(exportData);

            var fileName = $"relatorio_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.{formatter.GetFileExtension()}";

            return new ExportFileResultDto(
                fileContent,
                fileName,
                formatter.GetContentType()
            );
        }

        private string RenderHtmlTemplate(
                string templateName,
                string userName,
                string totalHours,
                string averageDailyHours,
                object barChartData,
                object donutChartData,
                string projectLegend,
             List<TimesheetExportInternalDto> timesheets)
        {
            var path = Path.Combine(AppContext.BaseDirectory,
                "Features",
                "TimesheetReports",
                "Templates",
                templateName
            );

            var html = File.ReadAllText(path);

            var projectBreakdown = PrepareProjectBreakdown(timesheets);
            var projectBreakdownHtml = GenerateProjectBreakdownHtml(projectBreakdown);

            var rows = new StringBuilder();

            html = html.Replace("{{UserName}}", HtmlHelper.Escape(userName));
            html = html.Replace("{{TotalHours}}", HtmlHelper.Escape(totalHours));
            html = html.Replace("{{AverageDailyHours}}", HtmlHelper.Escape(averageDailyHours));
            html = html.Replace("{{DateRange}}",
                $"{timesheets.First().StartDate:dd/MM/yyyy} - {timesheets.Last().StopDate:dd/MM/yyyy}");

            html = html.Replace("{{BarChartData}}", HtmlHelper.ToJsonSafe(barChartData));
            html = html.Replace("{{DonutChartData}}", HtmlHelper.ToJsonSafe(donutChartData));
            html = html.Replace("{{ProjectLegend}}", projectLegend);
            html = html.Replace("{{ROWS}}", rows.ToString());
            html = html.Replace("{{PROJECT_BREAKDOWN}}", projectBreakdownHtml);

            return html;
        }

        private object PrepareBarChartData(List<TimesheetExportInternalDto> timesheets)
        {
            var grouped = timesheets
                .GroupBy(t => t.StartDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalHours = g.Sum(t => ParseDuration(t.Duration))
                })
                .ToList();

            return new
            {
                labels = grouped.Select(g => g.Date.ToString("ddd dd/MM")).ToArray(),
                values = grouped.Select(g => g.TotalHours).ToArray(),
                colors = grouped.Select(_ => "#1C3661").ToArray()
            };
        }

        private object PrepareDonutChartData(List<TimesheetExportInternalDto> timesheets)
        {
            var grouped = timesheets
                .GroupBy(t => new
                {
                    ProjectKey = t.ProjectId ?? t.Project,
                    ProjectName = t.Project ?? "Sem Projeto"
                })
                .Select(g => new
                {
                    Project = g.Key.ProjectName,
                    Hours = g.Sum(t => ParseDuration(t.Duration)),
                    Color = g
                        .Select(t => t.BlockColor)
                        .FirstOrDefault(c => !string.IsNullOrEmpty(c)) ?? "#3B5C7E"
                })
                .ToList();

            var totalHours = grouped.Sum(g => g.Hours);

            if (totalHours == 0)
            {
                return new
                {
                    labels = new[] { "Sem dados" },
                    values = new[] { 100.0 },
                    colors = new[] { "#cccccc" }
                };
            }

            var result = grouped
                .Select(g => new
                {
                    g.Project,
                    g.Color,
                    g.Hours,
                    Percentage = Math.Round((g.Hours / totalHours) * 100, 1)
                })
                .OrderByDescending(g => g.Hours)
                .ToList();

            return new
            {
                labels = result.Select(r => r.Project).ToArray(),
                values = result.Select(r => r.Percentage).ToArray(),
                colors = result.Select(r => r.Color).ToArray()
            };
        }

        private List<ProjectBreakdownDto> PrepareProjectBreakdown(List<TimesheetExportInternalDto> timesheets)
        {
            var totalSeconds = timesheets.Sum(t => ParseDurationToSeconds(t.Duration));

            var grouped = timesheets
                .GroupBy(t => new
                {
                    ProjectKey = t.ProjectId ?? t.Project,
                    ProjectName = t.Project,
                    Color = t.BlockColor
                })
                .Select(g =>
                {
                    var projectSeconds = g.Sum(t => ParseDurationToSeconds(t.Duration));

                    return new ProjectBreakdownDto
                    {
                        ProjectName = g.Key.ProjectName,
                        ProjectColor = g.Key.Color,
                        Duration = FormatDuration(projectSeconds),
                        Percentage = Math.Round((double)projectSeconds / totalSeconds * 100, 2),
                        Items = g.Select(t => new DescriptionItemDto
                        {
                            Description = t.Description,
                            Duration = t.Duration,
                            Percentage = Math.Round(ParseDurationToSeconds(t.Duration) / (double)totalSeconds * 100, 2)
                        }).ToList()
                    };
                })
                .OrderByDescending(p => ParseDurationToSeconds(p.Duration))
                .ToList();

            return grouped;
        }
        private int ParseDurationToSeconds(string duration)
        {
            var parts = duration.Split(':');
            var hours = int.Parse(parts[0]);
            var minutes = int.Parse(parts[1]);
            var seconds = parts.Length > 2 ? int.Parse(parts[2]) : 0;
            return (hours * 3600) + (minutes * 60) + seconds;
        }

        private string FormatDuration(int totalSeconds)
        {
            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds % 3600) / 60;
            var seconds = totalSeconds % 60;
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        private string GenerateProjectBreakdownHtml(List<ProjectBreakdownDto> breakdown)
        {
            var sb = new StringBuilder();

            foreach (var project in breakdown)
            {
                sb.Append($"""
                    <div class="project-group">
                        <div class="project-row">
                            <div class="project-name">
                                <span class="bullet" style="background-color: {project.ProjectColor}">•</span>
                                {HtmlHelper.Escape(project.ProjectName)}
                            </div>
                            <div class="project-duration">{HtmlHelper.Escape(project.Duration)}</div>
                            <div class="project-percentage">{project.Percentage:F2}%</div>
                        </div>
                    """);

                foreach (var item in project.Items)
                {
                    sb.Append($"""
                        <div class="item-row">
                            <div class="item-description">{HtmlHelper.Escape(item.Description)}</div>
                            <div class="item-duration">{HtmlHelper.Escape(item.Duration)}</div>
                            <div class="item-percentage">{item.Percentage:F2}%</div>
                        </div>
                        """);
                }

                sb.Append("</div>");
            }

            return sb.ToString();
        }
        private string GenerateProjectLegend(dynamic donutData)
        {
            var sb = new StringBuilder();
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(donutData)
            );

            var labels = JsonSerializer.Deserialize<string[]>(data["labels"].ToString());
            var values = JsonSerializer.Deserialize<double[]>(data["values"].ToString());
            var colors = JsonSerializer.Deserialize<string[]>(data["colors"].ToString());

            const int maxProjects = 12;
            int projectsToShow = Math.Min(labels.Length, maxProjects);

            for (int i = 0; i < projectsToShow; i++)
            {
            sb.Append($"""
                <div class="legend-item">
                    <div class="legend-color" style="background-color: {colors[i]}"></div>
                    <div class="legend-details">
                    <span>{HtmlHelper.Escape(labels[i])}</span>
                    <span>{values[i]}%</span>
                    </div>
                </div>
                """);
            }

            if (labels.Length > maxProjects)
            {
                int remainingCount = labels.Length - maxProjects;

                sb.Append($"""
                    <div class="legend-item legend-others">
                        <div class="legend-details">
                        <span style="font-style: italic; color: #666;">e {remainingCount} {(remainingCount == 1 ? "outro" : "outros")}</span>
                        <span></span>
                        </div>
                    </div>
                 """);
            }
            return sb.ToString();
        }

        private double ParseDuration(string duration)
        {
            var parts = duration.Split(':');
            return double.Parse(parts[0]) + (double.Parse(parts[1]) / 60.0);
        }

        private string CalculateAverageDailyHours(List<TimesheetExportInternalDto> timesheets)
        {
            var days = timesheets.Select(t => t.StartDate.Date).Distinct().Count();
            var totalHours = timesheets.Sum(t => ParseDuration(t.Duration));
            var avg = totalHours / days;
            return $"{avg:F2} Horas";
        }

        private string CalculateDuration(DateTime start, DateTime? end)
        {
            if (!end.HasValue) return "00:00:00";

            var duration = end.Value - start;
            return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
        }

        private string CalculateTotalHours(List<TimesheetExportInternalDto> items)
        {
            var totalSeconds = items.Sum(i => { 
                var parts = i.Duration.Split(':'); 
                var hours = int.Parse(parts[0]); 
                var minutes = int.Parse(parts[1]);
                var seconds = int.Parse(parts[2]) ;
                return (hours * 3600) + (minutes * 60) + seconds;
            }); 

            var hoursTotal = totalSeconds / 3600; 
            var minutesTotal = (totalSeconds % 3600) / 60;
            var secondsTotal = totalSeconds % 60; 

            return $"{hoursTotal:D2}:{minutesTotal:D2}:{secondsTotal:D2}";
        }

        private string GetBlockColor(Domain.Entities.Timesheet timesheet)
        {
            if (!string.IsNullOrEmpty(timesheet.Project?.TitleColor))
            {
                return timesheet.Project.TitleColor;
            }

            if (!string.IsNullOrEmpty(timesheet.Customer?.Color))
            {
                return timesheet.Customer.Color;
            }

            if (!string.IsNullOrEmpty(timesheet.ActivityType?.ActivityTypeColor))
            {
                return timesheet.ActivityType.ActivityTypeColor;
            }
            return "#3B5C7E";
        }
    }
}