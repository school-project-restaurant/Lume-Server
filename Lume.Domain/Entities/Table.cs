namespace Lume.Domain.Entities;

public class Table
{
    public Guid Number { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int Seats { get; set; }
}