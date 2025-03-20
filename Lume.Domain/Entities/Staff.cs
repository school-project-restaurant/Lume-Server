using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class Staff : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public override string PhoneNumber { get; set; } = null!;
    public override string PasswordHash { get; set; } = null!;
    public int Salary { get; set; }
    public bool IsActive { get; set; }
    public int MonthHours { get; set; }
}