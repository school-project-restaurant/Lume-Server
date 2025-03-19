namespace Lume.Domain.Entities;

public class Table
{
    public Guid Number { get; set; }
    public List<int>? ReservationsId { get; set; }
    public int Seats { get; set; }
}