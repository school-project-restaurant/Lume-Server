using Lume.Domain.Constants;
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
    
    public async Task<(IEnumerable<ApplicationUser>, int)> GetMatchingCustomers(CustomerFilterOptions filterOptions, CustomerSortOptions sortOptions)
    {
        var query = dbContext.Users.Where(u => u.UserType == "Customer");
        query = ApplySearchFilters(query, filterOptions);
        query = sortOptions.SortBy is not null 
            ? ApplySorting(query, sortOptions.SortBy, sortOptions.SortDirection) 
            : query.OrderBy(u => u.Id);

        int totalCount = await query.CountAsync();
        var customers = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageIndex - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (customers, totalCount);
    }

    private IQueryable<ApplicationUser> ApplySearchFilters(IQueryable<ApplicationUser> query,
        CustomerFilterOptions options)
    { // Consider use normalized fields
        if (options.SearchEmail != null)
            query = query.Where(u => u.Email != null && u.Email.Contains(options.SearchEmail));
    
        if (options.SearchName != null)
            query = query.Where(u => u.Name.Contains(options.SearchName));
    
        if (options.SearchSurname != null)
            query = query.Where(u => u.Surname.Contains(options.SearchSurname));
    
        if (options.ReservationId != null)
            query = query.Where(u => u.ReservationsId != null && 
                                     u.ReservationsId.Any(id => id == options.ReservationId));
    
        if (options.SearchPhone != null)
            query = query.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(options.SearchPhone));
    
        return query;
    }
    
    private IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, string sortBy, SortDirection? direction)
    {
        bool isDescending = direction == SortDirection.Descending;
    
        return sortBy.ToLower() switch
        {
            "id" => isDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
            "name" => isDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
            "surname" => isDescending ? query.OrderByDescending(u => u.Surname) : query.OrderBy(u => u.Surname),
            "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "phonenumber" => isDescending ? query.OrderByDescending(u => u.PhoneNumber) : query.OrderBy(u => u.PhoneNumber),
            // Add other properties as needed
            _ => throw new KeyNotFoundException($"Property {sortBy} not found or not sortable")
        };
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