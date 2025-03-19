using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class Customer : IdentityUser
{
    public override string Id { get; set; }
    public override string? Email { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public override string PhoneNumber { get; set; } = string.Empty;
    public List<int>? ReservationsId { get; set; }
    public override string? PasswordHash { get; set; }
}