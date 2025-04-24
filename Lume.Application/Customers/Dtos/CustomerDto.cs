namespace Lume.Application.Customers.Dtos;

public class CustomerDto
{

    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
    
    public List<Guid>? ReservationsId { get; set; }

}