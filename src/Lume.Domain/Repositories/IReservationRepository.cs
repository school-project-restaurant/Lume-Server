using Lume.Domain.Constants;
using Lume.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lume.Domain.Repositories;

/// <summary>
/// Public repository for managing reservations
/// </summary>
public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllReservations();
    Task<(IEnumerable<Reservation>, int)> GetMatchingReservations(ReservationFilterOptions filterOptions, ReservationSortOptions sortOptions);
    Task<Reservation?> GetReservationById(Guid id);
    Task<Guid> CreateReservation(Reservation reservation);
    Task DeleteReservation(Reservation reservation);
    Task SaveChanges();
}

public class ReservationFilterOptions
{
    public Guid? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public int? PartySize { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}

public class ReservationSortOptions
{
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}
