namespace Lume.Infrastructure.Persistence.Seeders.Models;

public class TableSeedDataModel
{
    public int Number { get; set; }
    public IEnumerable<string> ReservationsId { get; set; } = Array.Empty<string>();
    public int Seats { get; set; }
}