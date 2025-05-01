using Lume.Application.Dishes.Dtos;
using MediatR;

namespace Lume.Application.Dishes.Queries.GetAllDishes;

public class GetAllDishesQuery : IRequest<IEnumerable<DishDto>>
{

}
