using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationCommandHandler(ILogger<DeleteReservationCommandHandler> logger,
    IReservationRepository reservationRepository) : IRequestHandler<DeleteReservationCommand, bool>
{
    public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting reservation with id {ReservationGuid}", request.Id);
        var reservation = await reservationRepository.GetReservationById(request.Id);
        if (reservation is null) return false;

        await reservationRepository.DeleteReservation(reservation);
        return true;
    }
}
