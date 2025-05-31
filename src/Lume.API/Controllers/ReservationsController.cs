using Lume.Application.Common;
using Lume.Application.Reservations.Commands.CreateReservation;
using Lume.Application.Reservations.Commands.DeleteReservation;
using Lume.Application.Reservations.Commands.UpdateReservation;
using Lume.Application.Reservations.Dtos;
using Lume.Application.Reservations.Queries.GetAllReservations;
using Lume.Application.Reservations.Queries.GetReservationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
// Customer role might be allowed to manage their own, but that requires additional logic
[Authorize(Roles = "Admin,Staff")]
public class ReservationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ReservationDto>>> GetAllReservations([FromQuery] GetAllReservationsQuery query) =>
        Ok(await mediator.Send(query));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationById([FromRoute] Guid id) =>
        Ok(await mediator.Send(new GetReservationByIdQuery(id)));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReservation(CreateReservationCommand command)
    {
        // Note: CustomerId is included in the command from the request body.
        // In a production app, you might want to get the user ID from the claims
        // if a customer is creating a reservation for themselves, to prevent spoofing.
        // E.g., command.CustomerId = User.GetUserId();

        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetReservationById), new { id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReservation([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteReservationCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReservation([FromRoute] Guid id, [FromBody] UpdateReservationCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    // Future endpoints might include:
    // [HttpGet("customer/{customerId}")]
    // public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByCustomerId([FromRoute] Guid customerId) { ... }
    // [Authorize(Roles = "Customer")]
    // [HttpGet("my")]
    // public async Task<ActionResult<IEnumerable<ReservationDto>>> GetMyReservations() { ... }
}
