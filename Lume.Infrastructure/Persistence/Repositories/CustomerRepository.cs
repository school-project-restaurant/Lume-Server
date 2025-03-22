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

    public async Task<Customer?> GetCustomerById(Guid id)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        return customer;
    }

    public async Task<Guid> CreateCustomer(Customer customer)
    {
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();
        return customer.Id;
    }

    public async Task DeleteCustomer(Customer customer)
    {
        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}