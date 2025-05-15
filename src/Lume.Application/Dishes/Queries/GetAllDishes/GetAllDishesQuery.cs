using Lume.Application.Common;
using Lume.Application.Dishes.Dtos;
using Lume.Domain.Constants;
using MediatR;

namespace Lume.Application.Dishes.Queries.GetAllDishes;

public class GetAllDishesQuery : IRequest<PagedResult<DishDto>>
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? Ingredients { get; set; }
    
    public int PageSize { get; set; } = 10; // Default page size
    public int PageIndex { get; set; } = 1; // Default page index
    
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}
