using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class CustomerRepository(RestaurantDbContext dbContext) : ICustomerRepository
{
    public async Task<IEnumerable<ApplicationUser>> GetAllCustomers()
    {
        var customers = await dbContext.Users.Where(u => 
            u.UserType == "Customer").ToListAsync();
        return customers;
    }

    public async Task<ApplicationUser?> GetCustomerById(Guid id)
    {
        var customer = await dbContext.Users.FirstOrDefaultAsync(u => 
            u.UserType == "Customer" && u.Id == id);
        return customer;
    }

    public async Task<Guid> CreateCustomer(ApplicationUser customer)
    {
        dbContext.Users.Add(customer);
        await dbContext.SaveChangesAsync();
        return customer.Id;
    }

    public async Task DeleteCustomer(ApplicationUser customer)
    {
        dbContext.Users.Remove(customer);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}