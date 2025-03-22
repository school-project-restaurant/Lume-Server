using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer?> GetCustomerById(Guid id);
    Task<Guid> CreateCustomer(Customer customer);
    Task DeleteCustomer(Customer customer);
    Task SaveChanges();
}