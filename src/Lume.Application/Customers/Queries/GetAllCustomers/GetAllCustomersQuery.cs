using Lume.Application.Customers.Dtos;
using MediatR;

namespace Lume.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    
}