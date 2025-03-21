using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer?> GetCustomerById(int id);
    Task<int> CreateCustomer(Customer customer);
    Task DeleteCustomer(Customer customer);
}