namespace Lume.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; } = ReservationStatus.Pending;
    public string? Notes { get; set; }
}

public static class ReservationStatus
{
    public const string Confirmed = "Confirmed";
    public const string Rejected = "Rejected";
    public const string Pending = "Pending";

    private static readonly IReadOnlyCollection<string> AllStatuses = 
        new[] { Confirmed, Rejected, Pending };
        
    public static bool IsValid(string status) => 
        AllStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
}