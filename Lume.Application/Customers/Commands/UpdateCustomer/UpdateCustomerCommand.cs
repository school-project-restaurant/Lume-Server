using MediatR;

namespace Lume.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand() : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
}