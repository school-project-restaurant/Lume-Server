using Lume.Application.Reservations.Dtos;
using MediatR;

namespace Lume.Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQuery(Guid id) : IRequest<ReservationDto>
{
    public Guid Id { get; set; } = id;
}
