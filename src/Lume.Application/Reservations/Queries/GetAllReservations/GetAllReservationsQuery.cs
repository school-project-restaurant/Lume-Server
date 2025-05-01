using Lume.Application.Reservations.Dtos;
using MediatR;

namespace Lume.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQuery : IRequest<IEnumerable<ReservationDto>>
{

}
