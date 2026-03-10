using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class UpdateTimesheetQueryHandlerTests
    {

        private readonly IMapper _mapper;

        public UpdateTimesheetQueryHandlerTests() {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task UpdateTimesheetWhenAvaiable()
        {
            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var fakeTimesheet = new Timesheet
            {
                TimesheetId = timesheetId,
                StartDate = startDate,
                EndDate = endDate,
                Description = "Descrição teste",
                UserId = userId
            };

            var mockRepo = new Mock<ITimesheetRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);
            mockRepo.Setup(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateTimesheetCommand(startDate, endDate, fakeTimesheet.Description, timesheetId, userId);
            var handler = new UpdateTimesheetCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetId, result.TimesheetId);
            Assert.Equal("Descrição teste", result.Description);

            mockRepo.Verify(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateTimesheetAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TimesheetShouldntUpdateWhenNotFound_ThrowNotFoundException()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 01, 01);
            var endDate = new DateTime(2025, 01, 01);
            var fakeTimesheet = new Timesheet
            {
                TimesheetId = timesheetId,
                StartDate = startDate,
                EndDate = endDate,
                Description = "Descrição teste",
                UserId = userId
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(timesheetId, CancellationToken.None))
                .ThrowsAsync(new TimesheetNotFoundException($"Timesheet com id {timesheetId} não encontrado."));

            var query = new UpdateTimesheetCommand(startDate, endDate, fakeTimesheet.Description, timesheetId, userId);
            var handler = new UpdateTimesheetCommandHandler(mockRepository.Object, _mapper);

            await Assert.ThrowsAsync<TimesheetNotFoundException>(() =>
                handler.Handle(query, CancellationToken.None));
        }
    }
}
