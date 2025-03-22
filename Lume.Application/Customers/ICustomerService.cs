using Lume.Application.Customers.Dtos;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Entities;

namespace Lume.Application.Customers;

public interface ICustomerService
{
    Task<ReservationDto> GetReservations(int id);
    Task CreateReservation(int id, Reservation reservation);
}