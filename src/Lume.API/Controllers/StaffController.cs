using Lume.Application.Common;
using Lume.Application.Staff.Commands.CreateStaff;
using Lume.Application.Staff.Commands.DeleteStaff;
using Lume.Application.Staff.Commands.UpdateStaff;
using Lume.Application.Staff.Dtos;
using Lume.Application.Staff.Queries.GetAllStaff;
using Lume.Application.Staff.Queries.GetStaffById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<StaffDto>>> GetAllStaff([FromQuery] GetAllStaffQuery query) =>
        Ok(await mediator.Send(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById([FromRoute] Guid id) =>
        Ok(await mediator.Send(new GetStaffByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Post(CreateStaffCommand command)
    {
        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetStaffById), new { id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStaff([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteStaffCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStaff([FromRoute] Guid id, [FromBody] UpdateStaffCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
