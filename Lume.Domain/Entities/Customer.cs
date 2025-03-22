using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lume.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public List<Guid>? ReservationsId { get; set; }
    public string PasswordHash { get; set; } = null!;
}