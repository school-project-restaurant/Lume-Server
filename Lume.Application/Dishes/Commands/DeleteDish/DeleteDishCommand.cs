using MediatR;

namespace Lume.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; set; } = id;
}
