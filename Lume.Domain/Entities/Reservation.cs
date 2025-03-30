namespace Lume.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public Status Status { get; set; }
    public string? Notes { get; set; }
}

public enum Status
{
    Confirmed,
    Rejected,
    Pending
}