namespace Lume.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int CustomersCount { get; set; }
    public Customer Customer { get; set; } = null!;
    public Status Status { get; set; }
    
    public string? Notes { get; set; }
}

public enum Status
{
    Confirmed,
    Rejected,
    Pending
}