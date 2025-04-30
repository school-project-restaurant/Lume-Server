using Lume.Domain.Entities; // Needed for Reservation entity
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore; // Needed for ToListAsync, FirstOrDefaultAsync, etc.

namespace Lume.Infrastructure.Persistence.Repositories;

internal class ReservationRepository(RestaurantDbContext dbContext) : IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        var reservations = await dbContext.Reservations.ToListAsync();
        return reservations;
    }

    public async Task<Reservation?> GetReservationById(Guid id)
    {
        var reservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        // Alternatively, if you only need to find by PK:
        // var reservation = await dbContext.Reservations.FindAsync(id);
        return reservation;
    }

    public async Task<Guid> CreateReservation(Reservation reservation)
    {
        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync(); // Save changes to generate the ID
        return reservation.Id; // The ID is populated by EF Core after saving
    }

    public async Task DeleteReservation(Reservation reservation)
    {
        dbContext.Reservations.Remove(reservation);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
