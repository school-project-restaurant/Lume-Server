using Lume.Application.Common;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Constants;
using MediatR;

namespace Lume.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQuery : IRequest<PagedResult<ReservationDto>>
{
    public Guid? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public int? PartySize { get; set; }
    
    public int PageSize { get; set; } = 10; // Default page size
    public int PageIndex { get; set; } = 1; // Default page index
    
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}
