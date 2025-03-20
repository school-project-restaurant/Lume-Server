namespace Lume.Domain.Entities;

public class Table
{
    public int Number { get; set; }
    public List<int>? ReservationsId { get; set; }
    public int Seats { get; set; }
}