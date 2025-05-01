using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

/// <summary>
/// Public repository for managing prenotations
/// </summary>
public interface IReservationRepository
{

    Task<IEnumerable<Reservation>> GetAllReservations();
    Task<Reservation?> GetReservationById(Guid id);
    Task<Guid> CreateReservation(Reservation reservation);
    Task DeleteReservation(Reservation reservation);
    Task SaveChanges();
}
