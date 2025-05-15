using Lume.Domain.Constants;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class ReservationRepository(RestaurantDbContext dbContext) : IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        var reservations = await dbContext.Reservations.ToListAsync();
        return reservations;
    }

    public async Task<(IEnumerable<Reservation>, int)> GetMatchingReservations(ReservationFilterOptions filterOptions, ReservationSortOptions sortOptions)
    {
        var query = dbContext.Reservations.AsQueryable();
        query = ApplySearchFilters(query, filterOptions);
        query = sortOptions.SortBy is not null 
            ? ApplySorting(query, sortOptions.SortBy, sortOptions.SortDirection) 
            : query.OrderByDescending(r => r.Date);

        int totalCount = await query.CountAsync();
        var reservations = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageIndex - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (reservations, totalCount);
    }

    private IQueryable<Reservation> ApplySearchFilters(IQueryable<Reservation> query, ReservationFilterOptions options)
    {
        if (options.CustomerId.HasValue)
            query = query.Where(r => r.CustomerId == options.CustomerId);
        
        if (options.FromDate.HasValue)
            query = query.Where(r => r.Date >= options.FromDate);
        
        if (options.ToDate.HasValue)
            query = query.Where(r => r.Date <= options.ToDate);
        
        if (!string.IsNullOrEmpty(options.Status))
            query = query.Where(r => r.Status.Contains(options.Status));
        
        if (options.PartySize.HasValue)
            query = query.Where(r => r.GuestCount == options.PartySize);
        
        return query;
    }
    
    private IQueryable<Reservation> ApplySorting(IQueryable<Reservation> query, string sortBy, SortDirection? direction)
    {
        bool isDescending = direction == SortDirection.Descending;
    
        return sortBy.ToLower() switch
        {
            "id" => isDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
            "customerid" => isDescending ? query.OrderByDescending(r => r.CustomerId) : query.OrderBy(r => r.CustomerId),
            "date" => isDescending ? query.OrderByDescending(r => r.Date) : query.OrderBy(r => r.Date),
            "status" => isDescending ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
            "guestcount" => isDescending ? query.OrderByDescending(r => r.GuestCount) : query.OrderBy(r => r.GuestCount),
            "tablenumber" => isDescending ? query.OrderByDescending(r => r.TableNumber) : query.OrderBy(r => r.TableNumber),
            "notes" => isDescending ? query.OrderByDescending(r => r.Notes) : query.OrderBy(r => r.Notes),
            _ => isDescending ? query.OrderByDescending(r => r.Date) : query.OrderBy(r => r.Date) // Default sort
        };
    }

    public async Task<Reservation?> GetReservationById(Guid id)
    {
        var reservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        return reservation;
    }

    public async Task<Guid> CreateReservation(Reservation reservation)
    {
        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync();
        return reservation.Id;
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
