using AutoMapper;
using Lume.Application.Customers.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(ILogger<GetCustomerByIdQueryHandler> logger, IMapper mapper,
    ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting customer with id {@Id}", request.Id);
        var customer = await customerRepository.GetCustomerById(request.Id);
        
        var customerDto = mapper.Map<CustomerDto>(customer);
        return customerDto;
    }
}