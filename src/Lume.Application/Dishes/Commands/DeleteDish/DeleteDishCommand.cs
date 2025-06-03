using MediatR;

namespace Lume.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}
