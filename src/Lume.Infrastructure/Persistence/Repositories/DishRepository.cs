using Lume.Domain.Constants;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class DishRepository(RestaurantDbContext dbContext) : IDishRepository
{
    public async Task<IEnumerable<Dish>> GetAllDishes()
    {
        return await dbContext.Dishes.ToListAsync();
    }

    public async Task<(IEnumerable<Dish>, int)> GetMatchingDishes(DishFilterOptions filterOptions, DishSortOptions sortOptions)
    {
        var query = dbContext.Dishes.AsQueryable();
        query = ApplySearchFilters(query, filterOptions);
        query = sortOptions.SortBy is not null 
            ? ApplySorting(query, sortOptions.SortBy, sortOptions.SortDirection) 
            : query.OrderBy(d => d.Id);

        int totalCount = await query.CountAsync();
        var dishes = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageIndex - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (dishes, totalCount);
    }

    private IQueryable<Dish> ApplySearchFilters(IQueryable<Dish> query, DishFilterOptions options)
    {
        if (options.MinPrice != null)
            query = query.Where(d => d.Price >= options.MinPrice);
    
        if (options.MaxPrice != null)
            query = query.Where(d => d.Price <= options.MaxPrice);
        
        if (options.Ingredients != null)
            query = query.Where(d => d.Ingredients == options.Ingredients);
    
        return query;
    }
    
    private IQueryable<Dish> ApplySorting(IQueryable<Dish> query, string sortBy, SortDirection? direction)
    {
        bool isDescending = direction == SortDirection.Descending;
    
        return sortBy.ToLower() switch
        {
            "id" => isDescending ? query.OrderByDescending(d => d.Id) : query.OrderBy(d => d.Id),
            "ingredients" => isDescending ? query.OrderByDescending(d => d.Ingredients) : query.OrderBy(d => d.Ingredients),
            "price" => isDescending ? query.OrderByDescending(d => d.Price) : query.OrderBy(d => d.Price),
            _ => throw new KeyNotFoundException($"Property {sortBy} not found or not sortable")
        };
    }

    public async Task<Dish?> GetDishById(Guid id)
    {
        return await dbContext.Dishes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Guid> CreateDish(Dish dish)
    {
        await dbContext.Dishes.AddAsync(dish);
        await dbContext.SaveChangesAsync();
        return dish.Id;
    }

    public async Task DeleteDish(Dish dish)
    {
        dbContext.Dishes.Remove(dish);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
