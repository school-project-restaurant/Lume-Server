using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommandHandler> logger, 
    ICustomerRepository customerRepository) : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting customer with id {CustomerId}", request.Id);
        var customer = await customerRepository.GetCustomerById(request.Id);
        if (customer is null) return false;
        
        await customerRepository.DeleteCustomer(customer);
        return true;
    }
}