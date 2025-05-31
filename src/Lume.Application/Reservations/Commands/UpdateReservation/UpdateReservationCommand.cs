using MediatR;

namespace Lume.Application.Reservations.Commands.UpdateReservation;

public class UpdateReservationCommand : IRequest
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
}
