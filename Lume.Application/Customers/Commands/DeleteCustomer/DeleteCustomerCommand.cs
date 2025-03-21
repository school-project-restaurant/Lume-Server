using MediatR;

namespace Lume.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}