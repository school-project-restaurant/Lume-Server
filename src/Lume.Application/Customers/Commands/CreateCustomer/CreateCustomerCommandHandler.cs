using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, IMapper mapper,
    ICustomerRepository customerRepository) : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new customer {@Customer}", request); // TODO exclude sensitive data from logs
        var customer = mapper.Map<ApplicationUser>(request);
        Guid id = await customerRepository.CreateCustomer(customer);
        return id;
    }
}