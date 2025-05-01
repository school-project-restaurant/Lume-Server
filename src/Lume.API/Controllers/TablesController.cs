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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TablesDto>>> GetAllTables()
    {
        var tables = await mediator.Send(new GetAllTableQuery());
        return Ok(tables);
    }

    [HttpGet("{number}")]
    public async Task<ActionResult<TablesDto>> GetTableByNumber(int number)
    {
        var table = await mediator.Send(new GetTableByNumberQuery(number));
        
        if (table == null)
            return NotFound();
            
        return Ok(table);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTable([FromBody] CreateTableCommand command)
    {
        var tableNumber = await mediator.Send(command);
        return CreatedAtAction(nameof(GetTableByNumber), new { number = tableNumber }, tableNumber);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> UpdateTable([FromBody] UpdateTableCommand command)
    {
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return Ok(result);
    }

    [HttpDelete("{number}")]
    public async Task<ActionResult<bool>> DeleteTable(int number)
    {
        var result = await mediator.Send(new DeleteTableCommand(number));
        
        if (!result)
            return NotFound();
            
        return Ok(result);
    }
}
