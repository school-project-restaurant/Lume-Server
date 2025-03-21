using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class CustomerRepository(RestaurantDbContext dbContext) : ICustomerRepository
{
    public Task<IEnumerable<Customer>> GetAllCustomers()
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetCustomerById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}