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
        CustomerFilterOptions filterOptions = mapper.Map<CustomerFilterOptions>(request);
        CustomerSortOptions sortOptions = mapper.Map<CustomerSortOptions>(request);
        var (customers, totalCount) = await customerRepository.GetMatchingCustomers(filterOptions, sortOptions);
        
        var customersDtos = mapper.Map<IEnumerable<CustomerDto>>(customers);
        var result = new PagedResult<CustomerDto>(customersDtos, totalCount, request.PageSize, request.PageIndex);
        return result;
    }
}