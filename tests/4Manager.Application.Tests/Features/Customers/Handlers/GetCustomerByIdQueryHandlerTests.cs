using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Features.Customers.Handlers;
using _4Tech._4Manager.Application.Features.Customers.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using Xunit;

namespace _4Tech._4Manager.Application.Tests.Features.Customers.Handlers
{
    public class GetCustomerByIdQueryHandlerTests
    {
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CustomerExists_ReturnsCustomerResponseDto()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customerId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var customer = new Customer
            {
                CustomerId = customerId,
                Name = "João Silva",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(customerId, cancellationToken))
                .ReturnsAsync(customer);

            var handler = new GetCustomerByIdQueryHandler(mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetCustomerByIdQuery(customerId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal("João Silva", result.Name);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task CustomerDoesNotExist_ThrowsCustomerNotFoundException()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customerId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId, cancellationToken))
                .ThrowsAsync(new CustomerNotFoundException($"Cliente com id {customerId} não encontrado."));

            var handler = new GetCustomerByIdQueryHandler(customerRepositoryMock.Object, _mapper);
            var query = new GetCustomerByIdQuery(customerId);

            await Assert.ThrowsAsync<CustomerNotFoundException>(() =>
                handler.Handle(query, CancellationToken.None));
        }
    }
}

