using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.DeleteCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Application.Customers.Queries.GetAllCustomers;
using Lume.Application.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var customers = await mediator.Send(new GetAllCustomersQuery());
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var customer = await mediator.Send(new GetCustomerByIdQuery(id));
        if (customer is null)
            return NotFound("Customer not found");

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateCustomerCommand command)
    {
        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var isDeleted = await mediator.Send(new DeleteCustomerCommand(id));
        if (isDeleted)
            return NoContent();

        return NotFound("Customer not found");
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateCustomerCommand command)
    {
        command.Id = id;
        var isUpdated = await mediator.Send(command);
        if (!isUpdated)
            return NotFound("Customer not found");

        return NoContent();
    }

    /*[HttpGet("{id}/reservations")]
    public async Task<IActionResult> GetClientReservations([FromRoute] int id)
    {
        var reservations = await customerService.GetReservations(id);
        return Ok(reservations);
    }

    [HttpPost("{id}/reservations")]
    public async Task<IActionResult> PostReservation([FromRoute] int id, [FromBody] ReservationDto reservationDto)
    {
        var reservation = ReservationDto.FromDto(reservationDto);
        await customerService.CreateReservation(id, reservation);

        return Created(nameof(GetClientReservations), reservation);
    }*/
}