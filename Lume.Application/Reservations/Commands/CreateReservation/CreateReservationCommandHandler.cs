using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler(ILogger<CreateReservationCommandHandler> logger, IMapper mapper,
    IReservationRepository reservationRepository) : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new reservation {@Reservation}", request); // TODO exclude sensitive data from logs
        var reservation = mapper.Map<Reservation>(request);
        var guid = await reservationRepository.CreateReservation(reservation);
        return guid;
    }
}
