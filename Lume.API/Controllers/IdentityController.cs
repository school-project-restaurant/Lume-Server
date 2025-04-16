using Lume.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost("userRole")]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}