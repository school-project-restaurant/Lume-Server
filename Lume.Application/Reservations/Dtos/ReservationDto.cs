using Lume.Domain.Entities;

namespace Lume.Application.Reservations.Dtos;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public static Reservation FromDto(ReservationDto reservationDto)
    {
        return new Reservation
        {
            Id = reservationDto.Id,
            Date = reservationDto.Date,
            Status = reservationDto.Status,
            Notes = reservationDto.Notes
        };
    }
}
