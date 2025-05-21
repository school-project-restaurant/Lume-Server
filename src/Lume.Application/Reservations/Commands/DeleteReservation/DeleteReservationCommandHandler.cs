using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Reservations.Commands.DeleteReservation;

public class DeleteReservationCommandHandler(ILogger<DeleteReservationCommandHandler> logger,
    IReservationRepository reservationRepository) : IRequestHandler<DeleteReservationCommand>
{
    public async Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting reservation with id {ReservationGuid}", request.Id);
        var reservation = await reservationRepository.GetReservationById(request.Id);
        if (reservation is null)
            throw new NotFoundException(nameof(reservation), request.Id);

        await reservationRepository.DeleteReservation(reservation);
    }
}
