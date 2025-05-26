using AutoMapper;
using Lume.Application.Common;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler(ILogger<GetAllReservationsQueryHandler> logger, IMapper mapper,
    IReservationRepository reservationsRepository) : IRequestHandler<GetAllReservationsQuery, PagedResult<ReservationDto>>
{
    public async Task<PagedResult<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting reservations with pagination from repository {OperationName}", nameof(reservationsRepository.GetMatchingReservations));
        
        ReservationFilterOptions filterOptions = mapper.Map<ReservationFilterOptions>(request);
        ReservationSortOptions sortOptions = mapper.Map<ReservationSortOptions>(request);
        var (reservations, totalCount) = await reservationsRepository.GetMatchingReservations(filterOptions, sortOptions);
        
        var reservationDtos = mapper.Map<IEnumerable<ReservationDto>>(reservations);
        var result = new PagedResult<ReservationDto>(reservationDtos, totalCount, request.PageSize, request.PageIndex);
        
        return result;
    }
}
