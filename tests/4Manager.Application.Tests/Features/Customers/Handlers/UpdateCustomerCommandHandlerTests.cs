using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Customers.Commands;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Features.Customers.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using Xunit;

namespace _4Tech._4Manager.Application.Tests.Features.Customers.Handlers
{
    public class UpdateCustomerCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Update_Customer_And_Return_Dto()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                CustomerId = customerId,
                Name = "João Silva",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            };

            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateCustomerCommand(customerId, "João Silva Atualizado", false);
            var handler = new UpdateCustomerCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal("João Silva Atualizado", result.Name);
            Assert.False(result.IsActive);

            mockRepo.Verify(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Customer_Not_Found()
        {
            var customerId = Guid.NewGuid();
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomerNotFoundException($"Cliente com id {customerId} não encontrado."));

            var command = new UpdateCustomerCommand(customerId, "Nome Atualizado", true);
            var handler = new UpdateCustomerCommandHandler(mockRepo.Object, _mapper);

            await Assert.ThrowsAsync<CustomerNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

