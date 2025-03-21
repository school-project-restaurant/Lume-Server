using MediatR;

namespace Lume.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}