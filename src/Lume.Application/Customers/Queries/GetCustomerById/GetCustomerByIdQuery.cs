using Lume.Application.Customers.Dtos;
using MediatR;

namespace Lume.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery(Guid id) : IRequest<CustomerDto?>
{
    public Guid Id { get; set; } = id;
}