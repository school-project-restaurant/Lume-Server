using MediatR;

namespace Lume.Application.Plates.Commands.UpdatePlate;

public class UpdatePlateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Calories { get; set; }
    public List<string> Ingredients { get; set; } = null!;
}
