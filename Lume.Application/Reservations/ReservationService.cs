using Lume.Domain.Repositories;

namespace Lume.Application.Reservations;

/// <summary>
/// Service for managing prenotations
/// </summary>
/// <param name="prenotationRepository">Operations</param>
internal class ReservationService(IPrenotationRepository prenotationRepository) : IReservationService
{
    
}