using Lume.Application.Customers;
using Lume.Application.Customers.Dtos;
using Lume.Application.Reservations;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var customers = await customerService.GetAll();
        return Ok(customers);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
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
        await customerService.Create(customer);
            
        return Created(nameof(GetUserById), customer);
    }
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody] CustomerDto customerDto)
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
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var customer = await customerService.GetById(id);
        if (customer is null)
            return NotFound("Customer not found");
        
        await customerService.Delete(id);
        return NoContent();
    }
    [HttpGet("{id}/reservations")]
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
    }
}