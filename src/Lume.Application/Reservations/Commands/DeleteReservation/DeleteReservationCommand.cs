using MediatR;

namespace Lume.Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}
