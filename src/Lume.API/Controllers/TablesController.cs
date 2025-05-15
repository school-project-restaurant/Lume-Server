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
    public async Task<ActionResult<PagedResult<TablesDto>>> GetAllTables([FromQuery] GetAllTableQuery query)
    {
        var pagedTables = await mediator.Send(query);
        return Ok(pagedTables);
    }

    [HttpGet("{number}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TablesDto>> GetTableByNumber(int number)
    {
        var table = await mediator.Send(new GetTableByNumberQuery(number));
        
        if (table == null)
            return NotFound("Table not found");
            
        return Ok(table);
    }

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
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound("Table not found");
            
        return Ok(result);
    }

    [HttpDelete("{number}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteTable(int number)
    {
        var result = await mediator.Send(new DeleteTableCommand(number));
        
        if (!result)
            return NotFound("Table not found");
            
        return Ok(result);
    }
}
