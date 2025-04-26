using Lume.Application.Reservations.Dtos;

namespace Lume.Application.Reservations;

public interface IReservationService
{
    Task<ReservationDto> GetReservationById(Guid id);
    Task<Guid> CreateReservation(Guid Id, int Seats); // ???
}
