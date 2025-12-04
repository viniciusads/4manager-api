using _4Tech._4Manager.Domain.Entities;

namespace _4Tech._4Manager.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);
        Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken);
    }
}