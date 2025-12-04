using _4Tech._4Manager.Application.Interfaces;
using _4Manager.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Common.Exceptions;

namespace _4Manager.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));

            var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

            if (customer == null)
                throw new CustomerNotFoundException($"Cliente com id {id} não encontrado.");

            return customer;
        }

        public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "O cliente não pode ser nulo.");

            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "O cliente não pode ser nulo.");

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));

            var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

            if (customer == null)
                throw new CustomerNotFoundException($"Cliente com id {id} não encontrado.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}