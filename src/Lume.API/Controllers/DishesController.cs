using Lume.Application.Common;
using Lume.Application.Dishes.Commands.CreateDish;
using Lume.Application.Dishes.Commands.DeleteDish;
using Lume.Application.Dishes.Commands.UpdateDish;
using Lume.Application.Dishes.Dtos;
using Lume.Application.Dishes.Queries.GetAllDishes;
using Lume.Application.Dishes.Queries.GetDishById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Chef")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<DishDto>>> GetAllDishes([FromQuery] GetAllDishesQuery query) =>
        Ok(await mediator.Send(query));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDishById([FromRoute] Guid id)
    {
        var plate = await mediator.Send(new GetDishByIdQuery(id));
        if (plate is null)
            return NotFound($"Dish with ID {id} not found");

        return Ok(plate);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDish([FromBody] CreateDishCommand command)
    {
        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetDishById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid id)
    {
        var isDeleted = await mediator.Send(new DeleteDishCommand(id));
        if (isDeleted)
            return NoContent();

        return NotFound($"Dish with ID {id} not found");
    }
    
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDish([FromRoute] Guid id, [FromBody] UpdateDishCommand command)
    {
        command.Id = id;
        var isUpdated = await mediator.Send(command);
        if (isUpdated)
            return NoContent();

        return NotFound($"Plate with ID {id} not found");
    }
}
