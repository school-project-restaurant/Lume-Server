using Lume.Domain.Constants;
using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

/// <summary>
/// Public repository for managing dishes
/// </summary>
public interface IDishRepository
{
    Task<IEnumerable<Dish>> GetAllDishes();
    Task<(IEnumerable<Dish>, int)> GetMatchingDishes(DishFilterOptions filterOptions, DishSortOptions sortOptions);
    Task<Dish?> GetDishById(Guid id);
    Task<Guid> CreateDish(Dish dish);
    Task DeleteDish(Dish dish);
    Task SaveChanges();
}

public class DishFilterOptions
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? Ingredients { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}

public class DishSortOptions
{
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}
