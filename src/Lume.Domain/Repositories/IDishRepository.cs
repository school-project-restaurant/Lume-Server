using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

/// <summary>
/// Public repository for managing prenotations
/// </summary>
public interface IDishRepository
{

    Task<IEnumerable<Dish>> GetAllDishes();
    Task<Dish?> GetDishById(Guid id);
    Task<Guid> CreateDish(Dish dish);
    Task DeleteDish(Dish dish);
    Task SaveChanges();
}
