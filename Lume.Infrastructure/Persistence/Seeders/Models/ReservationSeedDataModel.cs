using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders.Models;

public class ReservationSeedDataModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public Status Status { get; set; }
    public string? Notes { get; set; }
}