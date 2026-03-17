using _4Tech._4Manager.Application.Features.ActivityTypes.Dtos;
using _4Tech._4Manager.Application.Features.ActivityTypes.Handler;
using _4Tech._4Manager.Application.Features.ActivityTypes.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Reflection.Metadata;

namespace _4Tech._4Manager.Application.Tests.Features.ActivityTypes.Handlers
{
    public class GetActivityTypesQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapper = new();
        private readonly ActivityTypeTestFixture _fixture;
        private readonly Mock<IActivityTypeRepository> _repositoryMock = new();
        private readonly GetActivityTypesQueryHandler _handler;

        public GetActivityTypesQueryHandlerTests()
        {
            _handler = new GetActivityTypesQueryHandler(
                _repositoryMock.Object,
                _mapper.Object
            );
            _fixture = new ActivityTypeTestFixture();
        }

        [Fact]
        public async Task GetActivityTypesWhenAvailable()
        {
            var cancellationToken = CancellationToken.None;

            var fakeActivityType = _fixture.GeneratesActivityType(2);

            var mappedResult = fakeActivityType
                .Select(x => new ActivityTypeResponseDto())
                .ToList();

            _repositoryMock
                .Setup(repo => repo.GetAllActivityTypesAsync(cancellationToken))
                .ReturnsAsync(fakeActivityType);

            _mapper
                .Setup(m => m.Map<IEnumerable<ActivityTypeResponseDto>>(fakeActivityType))
                .Returns(mappedResult);

            var result = await _handler.Handle(new GetActivityTypesQuery(), cancellationToken);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            _repositoryMock.Verify(r =>
                r.GetAllActivityTypesAsync(cancellationToken), Times.Once);

            _mapper.Verify(m =>
                m.Map<IEnumerable<ActivityTypeResponseDto>>(fakeActivityType), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
        {
            var cancellationToken = new CancellationToken();

            var activityTypes = new List<ActivityType>();
            var mappedResult = new List<ActivityTypeResponseDto>();

            _repositoryMock
                .Setup(r => r.GetAllActivityTypesAsync(cancellationToken))
                .ReturnsAsync(activityTypes);

            _mapper
                .Setup(m => m.Map<IEnumerable<ActivityTypeResponseDto>>(activityTypes))
                .Returns(mappedResult);

           
            var result = await _handler.Handle(new GetActivityTypesQuery(), cancellationToken);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            var cancellationToken = new CancellationToken();

            _repositoryMock
                .Setup(r => r.GetAllActivityTypesAsync(cancellationToken))
                .ReturnsAsync((IEnumerable<ActivityType>)null!);

            _mapper
                .Setup(m => m.Map<IEnumerable<ActivityTypeResponseDto>>(null!))
                .Returns((IEnumerable<ActivityTypeResponseDto>)null!);

            var result = await _handler.Handle(new GetActivityTypesQuery(), cancellationToken);

            result.Should().BeNull();
        }


    }
}
