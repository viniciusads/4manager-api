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
    public class GetCustomersQueryHandlerTests
    {
        private readonly IMapper _mapper;

        public GetCustomersQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnListOfCustomerResponseDto()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var fakeCustomers = new List<Customer>
            {
                new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    Name = "Customer One",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    Name = "Customer Two",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(fakeCustomers);

            var handler = new GetCustomersQueryHandler(mockRepository.Object, _mapper);

            var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Customer One");
            Assert.Contains(result, c => c.Name == "Customer Two");
        }

        [Fact]
        public async Task ReturnEmptyListWhenNoCustomers()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var handler = new GetCustomersQueryHandler(mockRepository.Object, _mapper);

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Customer>());

            var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnPagedCustomers_WhenPageNumberAndPageSizeAreProvided()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var fakeCustomers = new List<Customer>
            {
                new Customer { CustomerId = Guid.NewGuid(), Name = "Customer 1", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Customer { CustomerId = Guid.NewGuid(), Name = "Customer 2", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Customer { CustomerId = Guid.NewGuid(), Name = "Customer 3", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Customer { CustomerId = Guid.NewGuid(), Name = "Customer 4", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Customer { CustomerId = Guid.NewGuid(), Name = "Customer 5", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeCustomers);

            var handler = new GetCustomersQueryHandler(mockRepository.Object, _mapper);
            var query = new GetCustomersQuery { PageNumber = 2, PageSize = 2 };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ReturnThrowCustomerNotFoundException_WhenRepositoryFails()
        {
            var mockRepository = new Mock<ICustomerRepository>();

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomerNotFoundException("Erro ao buscar clientes."));

            var handler = new GetCustomersQueryHandler(mockRepository.Object, _mapper);

            await Assert.ThrowsAsync<CustomerNotFoundException>(() =>
                handler.Handle(new GetCustomersQuery(), CancellationToken.None));
        }
    }
}

