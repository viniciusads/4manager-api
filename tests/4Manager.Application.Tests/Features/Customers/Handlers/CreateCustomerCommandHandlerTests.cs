using _4Tech._4Manager.Application.Features.Customers.Commands;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Features.Customers.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace _4Tech._4Manager.Application.Tests.Features.Customers.Handlers
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _handler = new CreateCustomerCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_CreatesCustomer_WhenCommandIsValid()
        {
            var command = new CreateCustomerCommand("João Silva");
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                Name = command.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var customerDto = new CustomerResponseDto
            {
                CustomerId = customer.CustomerId.Value,
                Name = customer.Name,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            };

            _repositoryMock.Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()))
                .Returns(customerDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customerDto.CustomerId, result.CustomerId);
            Assert.Equal(customerDto.Name, result.Name);
            Assert.True(result.IsActive);

            _repositoryMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SetsIsActiveToTrue_WhenCreatingCustomer()
        {
            var command = new CreateCustomerCommand("Maria Santos");
            var customerDto = new CustomerResponseDto
            {
                CustomerId = Guid.NewGuid(),
                Name = command.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()))
                .Returns(customerDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenRepositoryFails()
        {
            var command = new CreateCustomerCommand("Erro");
            var cancellationToken = new CancellationToken();

            _repositoryMock
                .Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), cancellationToken))
                .ThrowsAsync(new Exception("Database error"));

            Func<Task> act = async () =>
                await _handler.Handle(command, cancellationToken);

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _mapperMock.Verify(m =>
                m.Map<CustomerResponseDto>(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenMapperFails()
        {
            var command = new CreateCustomerCommand("Erro Mapper");
            var cancellationToken = new CancellationToken();

            _repositoryMock
                .Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()))
                .Throws(new Exception("Mapping error"));

            Func<Task> act = async () =>
                await _handler.Handle(command, cancellationToken);

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Mapping error");
        }

        [Fact]
        public async Task Handle_ShouldUseCancellationToken()
        {
           
            var command = new CreateCustomerCommand("Token Test");
            var cancellationTokenSource = new CancellationTokenSource();

            _repositoryMock
                .Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), cancellationTokenSource.Token))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()))
                .Returns(new CustomerResponseDto());

            await _handler.Handle(command, cancellationTokenSource.Token);

            _repositoryMock.Verify(r =>
                r.AddCustomerAsync(It.IsAny<Customer>(), cancellationTokenSource.Token),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedDto()
        {
            // Arrange
            var command = new CreateCustomerCommand("Maria");
            var cancellationToken = new CancellationToken();

            var expectedDto = new CustomerResponseDto
            {
                CustomerId = Guid.NewGuid(),
                Name = "Maria",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(r => r.AddCustomerAsync(It.IsAny<Customer>(), cancellationToken))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<CustomerResponseDto>(It.IsAny<Customer>()))
                .Returns(expectedDto);

            var result = await _handler.Handle(command, cancellationToken);

            result.Should().BeEquivalentTo(expectedDto);
        }

    }
}

