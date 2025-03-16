using Lume.Domain.Entities;

namespace Lume.Application.Customers.Dtos;

public class CustomerDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    
    public List<int>? ReservationsId { get; set; }

    public static Customer FromDto(CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Name = customerDto.Name,
            Surname = customerDto.Surname,
            ReservationsId = customerDto.ReservationsId,
        };
    }
}