using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class CustomerRepository(RestaurantDbContext dbContext) : ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        var customers = await dbContext.Customers.ToListAsync();
        return customers;
    }

    public Task<Customer?> GetCustomerById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateCustomer(Customer customer)
    {
        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();
        return 4;
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