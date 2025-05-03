using Lume.Application.Common;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.DeleteCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Application.Customers.Dtos;
using Lume.Application.Customers.Queries.GetAllCustomers;
using Lume.Application.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Customer")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<CustomerDto>>> GetAllCustomers([FromRoute] GetAllCustomersQuery query)
    {
        var customers = await mediator.Send(query);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
    {
        var customer = await mediator.Send(new GetCustomerByIdQuery(id));
        if (customer is null)
            return NotFound("Customer not found");

        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(CreateCustomerCommand command)
    {
        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCustomerById), new { id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
    {
        var isDeleted = await mediator.Send(new DeleteCustomerCommand(id));
        if (isDeleted)
            return NoContent();

        return NotFound("Customer not found");
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerCommand command)
    {
        command.Id = id;
        var isUpdated = await mediator.Send(command);
        if (isUpdated)
            return NoContent();

        return NotFound("Customer not found");
    }
}