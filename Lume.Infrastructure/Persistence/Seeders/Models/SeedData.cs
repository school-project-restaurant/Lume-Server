namespace Lume.Infrastructure.Persistence.Seeders.Models;

public class SeedData
{
    public IEnumerable<CustomerSeedDataModel> Customers { get; init; } = [];
    public IEnumerable<StaffSeedDataModel> Staff { get; init; } =  [];
    public IEnumerable<TableSeedDataModel> Tables { get; set; } =  [];
    public IEnumerable<ReservationSeedDataModel> Reservations { get; set; } =  [];
}