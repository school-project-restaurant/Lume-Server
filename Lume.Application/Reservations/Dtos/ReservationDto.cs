using Lume.Domain.Entities;

namespace Lume.Application.Reservations.Dtos;

public class ReservationDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int CustomersCount { get; set; }
    public string CustomerId { get; set; } = null!;
    public Status Status { get; set; }
    
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