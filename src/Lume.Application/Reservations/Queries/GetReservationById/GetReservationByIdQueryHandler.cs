using AutoMapper;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryHandler(ILogger<GetReservationByIdQueryHandler> logger, IMapper mapper,
    IReservationRepository reservationRepository) : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting reservation with id {@Id}", request.Id);
        var reservation = await reservationRepository.GetReservationById(request.Id);
        if (reservation is null)
            throw new NotFoundException(nameof(reservation), request.Id);

        var reservationsDto = mapper.Map<ReservationDto>(reservation);
        return reservationsDto;
    }
}
