using MediatR;

namespace Lume.Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; set; } = id;
}
