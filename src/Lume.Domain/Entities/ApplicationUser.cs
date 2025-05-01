using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string Name { get; set; } = null!; // TODO extract requirements from validator to constants file
    public string Surname { get; set; } = null!;
    [Required]
    public override string? PasswordHash { get; set; }
    [Required]
    public override string? PhoneNumber { get; set; }
    public string UserType { get; set; } = null!;
    public int Salary { get; set; }
    public bool IsActive { get; set; }
    public int MonthHours { get; set; }
    public List<Guid>? ReservationsId { get; set; }
}