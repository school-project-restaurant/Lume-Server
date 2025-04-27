using MediatR;

namespace Lume.Application.Plates.Commands.DeletePlate;

public class DeletePlateCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; set; } = id;
}
