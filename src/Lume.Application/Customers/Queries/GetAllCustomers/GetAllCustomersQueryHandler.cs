using AutoMapper;
using Lume.Application.Common;
using Lume.Application.Customers.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryHandler(ILogger<GetAllCustomersQueryHandler> logger, IMapper mapper,
    ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomersQuery, PagedResult<CustomerDto>>
{
    public async Task<PagedResult<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all customers from repository {OperationName}", nameof(customerRepository.GetAllCustomers));
        var customers = await customerRepository.GetAllCustomers();
        
        var customersDtos = mapper.Map<IEnumerable<CustomerDto>>(customers);
        var result = new PagedResult<CustomerDto>(customersDtos, 5, request.PageSize, request.PageIndex);
        return customersDtos!;
    }
}