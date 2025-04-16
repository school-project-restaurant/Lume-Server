using AutoMapper;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, IMapper mapper,
    ICustomerRepository customerRepository) : IRequestHandler<UpdateCustomerCommand, bool>
{
    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating customer with id {CustomerId}", request.Id); 
        var customer = await customerRepository.GetCustomerById(request.Id);
        if (customer is null) return false;
        
        mapper.Map(request, customer);
        await customerRepository.SaveChanges();
        return true;
    }
}