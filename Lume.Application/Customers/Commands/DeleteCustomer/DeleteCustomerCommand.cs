using MediatR;

namespace Lume.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; set; } = id;
}