namespace Lume.Application.Reservations.Dtos;

public class ReservationDto
{
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; }
    
    public string? Notes { get; set; }
}