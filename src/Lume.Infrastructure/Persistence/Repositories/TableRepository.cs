using Lume.Domain.Constants;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class TableRepository(RestaurantDbContext dbContext) : ITablesRepository
{
    public async Task<IEnumerable<Table>> GetAllTables()
    {
        return await dbContext.Tables.ToListAsync();
    }

    public async Task<(IEnumerable<Table>, int)> GetMatchingTables(TableFilterOptions filterOptions, TableSortOptions sortOptions)
    {
        var query = dbContext.Tables.AsQueryable();
        query = ApplySearchFilters(query, filterOptions);
        query = sortOptions.SortBy is not null 
            ? ApplySorting(query, sortOptions.SortBy, sortOptions.SortDirection) 
            : query.OrderBy(t => t.Number); // Default sort by table number

        int totalCount = await query.CountAsync();
        var tables = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageIndex - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (tables, totalCount);
    }

    private IQueryable<Table> ApplySearchFilters(IQueryable<Table> query, TableFilterOptions options)
    {
        if (options.SearchNumber.HasValue)
            query = query.Where(t => t.Number == options.SearchNumber);
        
        if (options.SearchMinSeats.HasValue)
            query = query.Where(t => t.Seats >= options.SearchMinSeats);
        
        if (options.SearchMaxSeats.HasValue)
            query = query.Where(t => t.Seats <= options.SearchMaxSeats);
        
        return query;
    }
    
    private IQueryable<Table> ApplySorting(IQueryable<Table> query, string sortBy, SortDirection? direction)
    {
        bool isDescending = direction == SortDirection.Descending;
    
        return sortBy.ToLower() switch
        {
            "number" => isDescending ? query.OrderByDescending(t => t.Number) : query.OrderBy(t => t.Number),
            "seats" => isDescending ? query.OrderByDescending(t => t.Seats) : query.OrderBy(t => t.Seats),
            _ => isDescending ? query.OrderByDescending(t => t.Number) : query.OrderBy(t => t.Number) // Default sort
        };
    }

    public async Task<Table?> GetTableByNumber(int id)
    {
        return await dbContext.Tables.FirstOrDefaultAsync(t => t.Number == id);
    }

    public async Task<int> CreateTable(Table table)
    {
        await dbContext.Tables.AddAsync(table);
        await dbContext.SaveChangesAsync();
        return table.Number;
    }

    public async Task DeleteTable(Table table)
    {
        dbContext.Tables.Remove(table);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
