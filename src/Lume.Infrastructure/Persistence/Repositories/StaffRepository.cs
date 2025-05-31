using Lume.Domain.Constants;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Lume.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class StaffRepository(RestaurantDbContext dbContext, IDistributedCache cache) : IStaffRepository
{
    public async Task<IEnumerable<ApplicationUser>> GetAllStaff()
    {
        var staff = await dbContext.Users.Where(u => u.UserType == "Staff").ToListAsync();
        return staff;
    }

    public async Task<(IEnumerable<ApplicationUser>, int)> GetMatchingStaff(StaffFilterOptions filterOptions,
        StaffSortOptions sortOptions)
    {
        var query = dbContext.Users.Where(u => u.UserType == "Staff");
        query = ApplySearchFilters(query, filterOptions);
        query = sortOptions.SortBy is not null
            ? ApplySorting(query, sortOptions.SortBy, sortOptions.SortDirection)
            : query.OrderBy(u => u.Id);

        int totalCount = await query.CountAsync();
        var staff = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageIndex - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (staff, totalCount);
    }

    private IQueryable<ApplicationUser> ApplySearchFilters(IQueryable<ApplicationUser> query,
        StaffFilterOptions options)
    {
        if (options.SearchName != null)
            query = query.Where(u => u.Name.Contains(options.SearchName));

        if (options.SearchSurname != null)
            query = query.Where(u => u.Surname.Contains(options.SearchSurname));

        if (options.SearchPhone != null)
            query = query.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(options.SearchPhone));

        return query;
    }

    private IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, string sortBy,
        SortDirection? direction)
    {
        bool isDescending = direction == SortDirection.Descending;

        return sortBy.ToLower() switch
        {
            "id" => isDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
            "name" => isDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
            "surname" => isDescending ? query.OrderByDescending(u => u.Surname) : query.OrderBy(u => u.Surname),
            "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "phonenumber" => isDescending
                ? query.OrderByDescending(u => u.PhoneNumber)
                : query.OrderBy(u => u.PhoneNumber),
            _ => throw new KeyNotFoundException($"Property {sortBy} not found or not sortable")
        };
    }

    public async Task<ApplicationUser?> GetStaffById(Guid id)
    {
        var key = $"key-{id}";
        return await cache.GetOrSetAsync(
            key,
            async () =>
            {
                return await dbContext.Users.FirstOrDefaultAsync(x =>
                    x.Id == id && x.UserType == "Staff");
            },
            new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
    }

    public async Task<Guid> CreateStaff(ApplicationUser staff)
    {
        staff.UserType = "Staff";
        dbContext.Users.Add(staff);
        await dbContext.SaveChangesAsync();
        return staff.Id;
    }

    public async Task DeleteStaff(ApplicationUser staff)
    {
        dbContext.Users.Remove(staff);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}