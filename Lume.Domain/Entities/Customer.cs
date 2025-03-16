using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class Customer : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public override string PhoneNumber { get; set; } = string.Empty;
    public List<int>? Reservations { get; set; }
}