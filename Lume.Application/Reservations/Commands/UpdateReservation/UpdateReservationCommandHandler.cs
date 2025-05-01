using AutoMapper;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Commands.UpdateReservation;

public class UpdateReservationCommandHandler(ILogger<UpdateReservationCommandHandler> logger, IMapper mapper,
    IReservationRepository reservationRepository) : IRequestHandler<UpdateReservationCommand, bool>
{
    public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating reservation with id {ReservationId}", request.Id); // Use request.Id
        var reservation = await reservationRepository.GetReservationById(request.Id); // Use request.Id
        if (reservation is null) return false;

        mapper.Map(request, reservation);
        await reservationRepository.SaveChanges();
        return true;
    }
}
