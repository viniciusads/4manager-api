using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Features.Timesheets.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Timesheets.Handlers
{
    public class GetTimesheetByIdQueryHandlerTests
    {
        private readonly IMapper _mapper;


        public GetTimesheetByIdQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timesheet, TimesheetResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnListOfTimesheetByDateRangeResponseDto()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var fakeTimesheet = new Timesheet
            {
                TimesheetId = timesheetId,
                StartDate = new DateTime(2025, 01, 01),
                EndDate = new DateTime(2025, 01, 01),
                UserId = userId
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(timesheetId, CancellationToken.None))
                .ReturnsAsync(fakeTimesheet);

            var handler = new GetTimesheetByIdQueryHandler(mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetTimesheetByIdQuery(timesheetId, userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(timesheetId, result.TimesheetId);
        }

        [Fact]
        public async Task TimesheetDoesntExist_ThrowsTimesheetNotFoundException()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            var mockMapper = new Mock<IMapper>();

            var timesheetId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetByIdAsync(timesheetId, CancellationToken.None))
                .ThrowsAsync(new TimesheetNotFoundException($"Timesheet com id {timesheetId} não encontrado."));

            var handler = new GetTimesheetByIdQueryHandler(mockRepository.Object, _mapper);
            var query = new GetTimesheetByIdQuery(timesheetId, userId);

            await Assert.ThrowsAsync<TimesheetNotFoundException>(() =>
                handler.Handle(query, CancellationToken.None));
        }
    }
}