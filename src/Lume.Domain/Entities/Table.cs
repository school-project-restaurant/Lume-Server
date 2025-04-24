namespace Lume.Domain.Entities;

public class Table
{
    public int Number { get; set; }
    public IEnumerable<string>? ReservationsId { get; set; }
    public int Seats { get; set; }
}