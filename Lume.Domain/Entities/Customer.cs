using System.ComponentModel.DataAnnotations;

namespace Lume.Domain.Entities;

public class Customer : ApplicationUser
{
    public List<Guid>? ReservationsId { get; set; }
}