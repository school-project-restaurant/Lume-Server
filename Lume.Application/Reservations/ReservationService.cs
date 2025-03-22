using Lume.Domain.Repositories;

namespace Lume.Application.Reservations;

/// <summary>
/// Service for managing prenotations
/// </summary>
/// <param name="reservationRepository">Operations</param>
internal class ReservationService(IReservationRepository reservationRepository) : IReservationService
{
    
}