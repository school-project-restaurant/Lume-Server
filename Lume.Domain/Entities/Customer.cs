using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class Customer : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public override string? Email { get; set; }
    public override string PhoneNumber { get; set; } = null!;
    public List<Guid>? ReservationsId { get; set; }
    public override string PasswordHash { get; set; } = null!;
}