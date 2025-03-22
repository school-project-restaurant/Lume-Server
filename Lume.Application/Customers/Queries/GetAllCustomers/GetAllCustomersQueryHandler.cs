using AutoMapper;
using Lume.Application.Customers.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryHandler(ILogger<GetAllCustomersQueryHandler> logger, IMapper mapper,
    ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get all customers");
        var customers = await customerRepository.GetAllCustomers();
        
        var customersDtos = mapper.Map<IEnumerable<CustomerDto>>(customers);
        return customersDtos!;
    }
}