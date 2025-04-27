using MediatR;

namespace Lume.Application.Dishes.Commands.CreateDish;

public class CreateDishCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Calories { get; set; }
    public List<string> Ingredients { get; set; } = null!;
}
