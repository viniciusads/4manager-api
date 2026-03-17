using _4Tech._4Manager.Application.Features.TimesheetReports.Handlers;
using _4Tech._4Manager.Application.Features.TimesheetReports.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Moq;
using System.Text;


namespace _4Tech._4Manager.Application.Tests.Features.TimesheetReports.Handlers
{
    public class ExportTimesheetReportQueryHandlerTests
    {
        private readonly Mock<ITimesheetRepository> _timesheetRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IPdfGeneratorService> _pdfGeneratorMock = new();
        private readonly ExportTimesheetReportQueryHandler _handler;

        
        public ExportTimesheetReportQueryHandlerTests()
        {
            _handler = new ExportTimesheetReportQueryHandler(
                _timesheetRepoMock.Object, 
                _userRepoMock.Object,
                _pdfGeneratorMock.Object
                );
        }


        [Fact]
        public async Task Handle_ReturnsCsvFile_WhenFormatIsCsv()
        {
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 01, 01, 08, 00, 00);
            var endDate = new DateTime(2026, 01, 01, 10, 30, 00);


            var timesheet = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                Description = "Teste de export formato CSV",
                StartDate = startDate,
                EndDate = endDate,
                Project = new Project { ProjectName = "Projeto 4managerTest" },
                Customer = new Customer {Name = "Cliente 4managerTest" },
                ActivityType = new ActivityType {ActivityTypeName = "Desenvolvimento"},
                UserId = userId
            };

            _timesheetRepoMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { timesheet });      

            _userRepoMock
                .Setup(r => r.GetUserNameByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())) 
                .ReturnsAsync("João Pacheco");


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "csv");


            var result = await _handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.EndsWith(".csv", result.FileName, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("text/csv", result.ContentType);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Descricao,Duracao,Membro,Projeto,Cliente,TipoDeAtividade", content);
            Assert.Contains("Teste de export formato CSV", content);
            Assert.Contains("João Pacheco", content);
            Assert.Contains("02:30", content); 

        }

        [Fact]
        public async Task Handle_ReturnsTxtFile_WhenFormatsIsTxt()
        {
            
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 02, 02, 09, 00, 00);
            var endDate = new DateTime(2026, 02, 02, 11, 15, 00);

            var timesheet = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),           
                Description = "Teste de export formato TXT",
                StartDate = startDate,
                EndDate = endDate,
                Project = new Project { ProjectName = "4ManagerTest" },
                Customer = new Customer { Name = "Cliente 4managerTest" },
                ActivityType = new ActivityType { ActivityTypeName = "Reunião" },
                UserId = userId
             };


            _timesheetRepoMock
                    .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { timesheet });

            _userRepoMock
                  .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync("João Silva");


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "txt");

            
            var result = await _handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.EndsWith(".txt", result.FileName, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("text/plain", result.ContentType);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Descricao|Duracao|Membro|Projeto|Cliente|TipoDeAtividade", content);
            Assert.Contains("Teste de export formato TXT", content);
            Assert.Contains("João Silva", content);
            Assert.Contains("02:15", content);
        }

        [Fact]

        public async Task Handle_ReturnsPdfFile_WhenFormatsIsPdf() {        
            
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 03, 01, 08, 00, 00);
            var endDate = new DateTime(2026, 03, 01, 12, 00, 00);
            var timesheet = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                Description = "Teste de export formato PDF",
                StartDate = startDate,
                EndDate = endDate,
                Project = new Project { ProjectName = "Projeto PDF Test" },
                Customer = new Customer { Name = "Cliente PDF Test" },
                ActivityType = new ActivityType { ActivityTypeName = "Desenvolvimento" },
                UserId = userId
            };
            _timesheetRepoMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { timesheet });
            _userRepoMock
                .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync("Maria Oliveira");
            _pdfGeneratorMock
                .Setup(p => p.GenerateFromHtmlAsync(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes("PDF content"));

            var result = await _handler.Handle(new ExportTimesheetReportQuery(startDate, endDate, userId, "pdf"), CancellationToken.None);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.NotNull(result);
        }


        [Fact]
                          
        public async Task Handle_ReturnsFileWithHeaderOnly_WhenNotTimesheetsInPeriod() 
        {
            var userId    = Guid.NewGuid();
            var startDate = new DateTime(2026, 03, 03);
            var endDate   = new DateTime(2026, 03, 05);


            _timesheetRepoMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<Timesheet>());


            _userRepoMock
                .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync("Leticia Carvalho"); 


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "csv");

   
            var result = await _handler.Handle(request, CancellationToken.None); 


            Assert.NotNull(result);

            var content = Encoding.UTF8.GetString(result.FileContent); 
            Assert.Contains("Descricao,Duracao,Membro,Projeto,Cliente,TipoDeAtividade", content); 


            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Single(lines);
        }


        
        [Fact]
        public async Task Handle_ReturnsFileWithAllRows_WhenMultipleTimesheets()
        {
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 04, 01, 08, 00, 00);
            var endDate = new DateTime(2026, 04, 01, 10, 00, 00);


            var multipleTimesheets = new[]
            {
                new Timesheet
                {
                    TimesheetId = Guid.NewGuid(),
                    Description = "Primeira Tarefa",
                    StartDate = startDate,
                    EndDate = endDate,
                    Project = new Project { ProjectName = "Projeto Alpha" },
                    Customer = new Customer { Name = "Cliente Alpha"},
                    ActivityType = new ActivityType { ActivityTypeName = "Desenvolvimento"},
                    UserId = userId,
                },

                new Timesheet
                {
                   TimesheetId  = Guid.NewGuid(),
                   Description  = "Segunda Tarefa",
                   StartDate    = new DateTime(2026, 04, 02, 09, 00, 00),
                   EndDate      = new DateTime(2026, 04, 02, 11, 30, 00),
                   Project      = new Project      { ProjectName      = "Projeto Beta"   },
                   Customer     = new Customer     { Name             = "Cliente Beta"   },
                   ActivityType = new ActivityType { ActivityTypeName = "Reunião"        },
                   UserId       = userId
                }
            };


            _timesheetRepoMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(multipleTimesheets);



            _userRepoMock
                .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync("Diego Maciel Pacheco");


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "csv");


            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Primeira Tarefa", content);
            Assert.Contains("Segunda Tarefa", content);
            Assert.Contains("02:00", content); 
            Assert.Contains("02:30", content); 

            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);

        }



        [Fact]
        public async Task Handle_ReturnsDurationZero_WhenTimesheetHasNoEndDate()
        {
            
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 05, 01, 10, 00, 00);
            var endDate = new DateTime(2026, 05, 01, 18, 00, 00);

            var timesheet = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                Description = "Timer ainda ativo",
                StartDate = startDate,
                EndDate = null,
                Project = new Project { ProjectName = "Projeto 4managerTest" },
                Customer = new Customer { Name = "Cliente 4managerTest" },
                ActivityType = new ActivityType { ActivityTypeName = "Desenvolvimento" },
                UserId = userId
            };


            _timesheetRepoMock
                .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { timesheet });

            _userRepoMock
                .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync("Eurides Maciel Pacheco");


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "csv");

            
            var result = await _handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Timer ainda ativo", content);
            Assert.Contains("00:00", content); 
        }

        

        [Fact]
        public async Task Handle_ReturnsFallbackValues_WhenTimesheetHasNoProjectCustomerOrActivityType()
        {
            
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2026, 06, 01, 08, 00, 00);
            var endDate = new DateTime(2026, 06, 01, 09, 00, 00);

            var timesheet = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                Description = "Tarefa sem vínculos",
                StartDate = startDate,
                EndDate = endDate,
                Project = null,
                Customer = null,
                ActivityType = null,
                UserId = userId
            };


            _timesheetRepoMock
                 .Setup(r => r.GetByDateRangeAsync(startDate, endDate, userId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new[] { timesheet });

            _userRepoMock
                  .Setup(r => r.GetUserNameByIdAsync(userId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync("André Felipe");


            var request = new ExportTimesheetReportQuery(startDate, endDate, userId, "csv");

            
            var result = await _handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);

            var content = Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Tarefa sem vínculos", content);
            Assert.Contains("\"-\"", content);              
            Assert.Contains("\"Sem atividade\"", content);

        }
    }
}

    
