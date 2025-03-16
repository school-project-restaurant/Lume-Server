using Lume.Application.Customers.Dtos;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Entities;

namespace Lume.Application.Customers;

public interface ICustomerService
{
    public Task<IEnumerable<CustomerDto>> GetAll();
    Task<CustomerDto?> GetById(int id);
    Task Create(Customer customer);
    Task Update(int id, CustomerDto customer);
    Task Delete(int id);
    Task<ReservationDto> GetReservations(int id);
    Task CreateReservation(int id, Reservation reservation);
}