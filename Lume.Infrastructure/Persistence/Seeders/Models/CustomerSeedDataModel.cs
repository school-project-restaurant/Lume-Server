namespace Lume.Infrastructure.Persistence.Seeders.Models;

public class CustomerSeedDataModel : BaseSeedDataModel
{
    public string Email { get; set; } = string.Empty;
    public IEnumerable<Guid> ReservationsId { get; set; } = Array.Empty<Guid>();
}