using System.ComponentModel.DataAnnotations;

namespace Lume.Domain.Entities;

public class Customer : ApplicationUser
{
    [Required]
    public override string? Email { get; set; }
    public List<Guid>? ReservationsId { get; set; }
}