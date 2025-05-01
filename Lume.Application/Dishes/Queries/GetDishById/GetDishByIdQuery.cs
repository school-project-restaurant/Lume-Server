using Lume.Application.Dishes.Dtos;
using MediatR;

namespace Lume.Application.Dishes.Queries.GetDishById;

public class GetDishByIdQuery(Guid id) : IRequest<DishDto?>
{
    public Guid Id { get; set; } = id;
}
