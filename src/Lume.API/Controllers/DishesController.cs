// Lume-Server/Lume.API/Controllers/PlatesController.cs

using Lume.Application.Dishes.Commands.CreateDish;
using Lume.Application.Dishes.Commands.DeleteDish;
using Lume.Application.Dishes.Commands.UpdateDish;
using Lume.Application.Dishes.Dtos;
using Lume.Application.Dishes.Queries.GetAllDishes;
using Lume.Application.Dishes.Queries.GetDishById;
using MediatR;
using Microsoft.AspNetCore.Authorization; // Added for Authorization attributes
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
// Assuming management of plates (create, update, delete) is for Admins and Chefs
// Read operations (GetAll, GetById) might be anonymous or restricted depending on requirements.
// Let's make GetAll and GetById accessible to "Customer" too, or even AllowAnonymous if it's a public menu.
// For management actions (POST, PATCH, DELETE), restrict to Admin, Chef.
[Authorize(Roles = "Admin,Chef")] // Applied at controller level
public class DishesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all plates from the menu.
    /// Accessible to anyone (Anonymous).
    /// </summary>
    [HttpGet]
    [AllowAnonymous] // Allows access without authentication
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishes()
    {
        // No authorization check needed here due to [AllowAnonymous]
        var plates = await mediator.Send(new GetAllDishesQuery());
        return Ok(plates);
    }

    /// <summary>
    /// Retrieves a specific plate by its ID.
    /// Accessible to anyone (Anonymous).
    /// </summary>
    [HttpGet("{id:guid}")] // Use :guid constraint for route parameter type
    [AllowAnonymous] // Allows access without authentication
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDishById([FromRoute] Guid id)
    {
        // No authorization check needed here due to [AllowAnonymous]
        var plate = await mediator.Send(new GetDishByIdQuery(id));
        if (plate is null)
            return NotFound($"Plate with ID {id} not found");

        return Ok(plate);
    }

    /// <summary>
    /// Creates a new plate.
    /// Requires Admin or Chef role.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // For validation errors
    public async Task<IActionResult> CreateDish([FromBody] CreateDishCommand command)
    {
        // Validation is handled by FluentValidation behavior pipeline if configured
        Guid id = await mediator.Send(command);
        // Return 201 Created with the location of the new resource
        return CreatedAtAction(nameof(GetDishById), new { id }, new { id }); // Return the created ID in the body
    }

    /// <summary>
    /// Deletes a plate by its ID.
    /// Requires Admin or Chef role.
    /// </summary>
    [HttpDelete("{id:guid}")] // Use :guid constraint
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid id)
    {
        var isDeleted = await mediator.Send(new DeleteDishCommand(id));
        if (isDeleted)
            return NoContent(); // 204 indicates successful deletion with no content

        return NotFound($"Plate with ID {id} not found"); // 404 if the plate doesn't exist
    }

    /// <summary>
    /// Updates an existing plate by its ID.
    /// Requires Admin or Chef role.
    /// </summary>
    [HttpPatch("{id:guid}")] // Using PATCH as in CustomerController
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // For validation errors
    public async Task<IActionResult> UpdateDish([FromRoute] Guid id, [FromBody] UpdateDishCommand command)
    {
        // Set the ID from the route parameter onto the command object
        command.Id = id;

        // Validation is handled by FluentValidation behavior pipeline if configured
        var isUpdated = await mediator.Send(command);

        if (isUpdated)
            return NoContent(); // 204 indicates successful update with no content

        return NotFound($"Plate with ID {id} not found"); // 404 if the plate doesn't exist
    }
}
