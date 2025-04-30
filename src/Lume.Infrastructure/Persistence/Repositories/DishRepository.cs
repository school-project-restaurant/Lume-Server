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
