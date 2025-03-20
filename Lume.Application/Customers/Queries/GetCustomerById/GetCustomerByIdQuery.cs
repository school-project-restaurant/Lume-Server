using Lume.Application.Customers.Dtos;
using MediatR;

namespace Lume.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery(int id) : IRequest<CustomerDto?>
{
    public int Id { get; set; } = id;
}