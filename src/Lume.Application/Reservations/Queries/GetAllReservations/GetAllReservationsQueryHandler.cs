using AutoMapper;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler(ILogger<GetAllReservationsQueryHandler> logger, IMapper mapper,
    IReservationRepository reservationsRepository) : IRequestHandler<GetAllReservationsQuery, IEnumerable<ReservationDto>>
{
    public async Task<IEnumerable<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all reservation from repository {OperationName}", nameof(reservationsRepository.GetAllReservations));
        var reservation = await reservationsRepository.GetAllReservations();

        var reservationsDtos = mapper.Map<IEnumerable<ReservationDto>>(reservation);
        return reservationsDtos!;
    }
}
