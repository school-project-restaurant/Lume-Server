using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [Required]
    public override string? PasswordHash { get; set; }
    [Required]
    public override string? PhoneNumber { get; set; }
    public string UserType { get; set; } = null!;
}