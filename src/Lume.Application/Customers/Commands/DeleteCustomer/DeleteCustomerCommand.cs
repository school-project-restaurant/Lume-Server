using MediatR;

namespace Lume.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}