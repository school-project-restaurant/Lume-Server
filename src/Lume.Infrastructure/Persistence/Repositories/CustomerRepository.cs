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
    
    public async Task<(IEnumerable<ApplicationUser>, int)> GetMatchingCustomers(CustomerFilterOptions filterOptions)
    {
        var query = dbContext.Users.AsQueryable();
        query = query.Where(u => u.UserType == "Customer");
        query = ApplySearchFilters(query, filterOptions);
        
    }

    private IQueryable<ApplicationUser> ApplySearchFilters(IQueryable<ApplicationUser> query,
        CustomerFilterOptions options)
    { // Consider use normalized fields
        query = query.Where(u => u.Email != null && options.SearchEmail != null 
                                                 && u.Email.Contains(options.SearchEmail));
        query = query.Where(u => options.SearchName == null || u.Name.Contains(options.SearchName));
        query = query.Where(u => options.SearchSurname == null || u.Surname.Contains(options.SearchSurname));
        query = query.Where(u => options.ReservationId != null && u.ReservationsId != null 
                                                               && u.ReservationsId.Any(id => id == options.ReservationId));
        query = query.Where(u => options.SearchPhone != null && u.PhoneNumber != null 
                                 && u.PhoneNumber.Contains(options.SearchPhone));
        return query;
    }

    public async Task<ApplicationUser?> GetCustomerById(Guid id)
    {
        var customer = await dbContext.Users.FirstOrDefaultAsync(u => 
            u.UserType == "Customer" && u.Id == id);
        return customer;
    }

    public async Task<Guid> CreateCustomer(ApplicationUser customer)
    {
        customer.UserType = "Customer";
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