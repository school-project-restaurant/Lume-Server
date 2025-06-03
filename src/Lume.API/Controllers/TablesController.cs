using Lume.Application.Common;
using Lume.Application.Tables.Commands.CreateTable;
using Lume.Application.Tables.Commands.DeleteTable;
using Lume.Application.Tables.Commands.UpdateTable;
using Lume.Application.Tables.Dtos;
using Lume.Application.Tables.Queries.GetAllTables;
using Lume.Application.Tables.Queries.GetTableByNumber;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves tables with pagination, filtering and sorting support.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TablesDto>>> GetAllTables([FromQuery] GetAllTableQuery query) =>
        Ok(await mediator.Send(query));

    [HttpGet("{number}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TablesDto>> GetTableByNumber(int number) =>
        Ok(await mediator.Send(new GetTableByNumberQuery(number)));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateTable([FromBody] CreateTableCommand command)
    {
        var tableNumber = await mediator.Send(command);
        return CreatedAtAction(nameof(GetTableByNumber), new { number = tableNumber }, tableNumber);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> UpdateTable([FromBody] UpdateTableCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{number}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteTable(int number)
    {
        await mediator.Send(new DeleteTableCommand(number));
        return NoContent();
    }
}
