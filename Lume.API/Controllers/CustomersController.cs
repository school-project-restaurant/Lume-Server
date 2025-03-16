using Lume.Application.Customers;
using Lume.Application.Customers.Dtos;
using Lume.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await customerService.GetAll();
        return Ok(customers);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var customer = await customerService.GetById(id);
        if (customer is null)
            return NotFound("Customer not found");
        
        return Ok(customer);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CustomerDto customerDto)
    {
        var customer = CustomerDto.FromDto(customerDto);
        await customerService.Create(customerDto);
            
        return Created(nameof(GetById), customer);
    }
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] CustomerDto customerDto)
    {
        var customer = await customerService.GetById(id);
        if (customer is null)
            return NotFound("Customer not found");
        
        await customerService.Update(id, customerDto);
        return NoContent();
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var customer = await customerService.GetById(id);
        if (customer is null)
            return NotFound("Customer not found");
        
        await customerService.Delete(id);
        return NoContent();
    }
}