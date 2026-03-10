using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class StopTimerTimesheetCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public StopTimerTimesheetCommandHandlerTests() {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ShouldPatchEndDateAndDescription()
        {
            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var endDate = new DateTime(2025, 01, 01);
            var description = "Descrição - patch endDate";

            var fakeTimesheet = new Timesheet
            {
                TimesheetId = timesheetId,
                EndDate = endDate,
                Description = description,
                UserId = userId
            };

            var mockRepo = new Mock<ITimesheetRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);
            mockRepo.Setup(r => r.StopTimerTimesheetAsync(timesheetId, endDate, description, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeTimesheet);

            var command = new StopTimerTimesheetCommand(endDate, timesheetId, description, userId);
            var handler = new StopTimerTimesheetCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetId, result.TimesheetId);
            Assert.Equal(description, result.Description);

            mockRepo.Verify(r => r.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.StopTimerTimesheetAsync(timesheetId, endDate, description, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TimesheetShouldntPatchEndDateWhenNotFound_ThrowNotFoundException()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var endDate = new DateTime(2025, 01, 01);
            var description = "Descrição - patch endDate";

            var fakeTimesheet = new Timesheet
            {
                TimesheetId = timesheetId,
                EndDate = endDate,
                Description = description,
                UserId = userId
            };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(timesheetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TimesheetNotFoundException($"Timesheet com id {timesheetId} não encontrado."));

            var command = new StopTimerTimesheetCommand(endDate, timesheetId, description, userId);
            var handler = new StopTimerTimesheetCommandHandler(mockRepository.Object, mockMapper.Object);

            await Assert.ThrowsAsync<TimesheetNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
