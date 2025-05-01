using MediatR;

namespace Lume.Application.Tables.Commands.CreateTable;

public class CreateTableCommand : IRequest<int>
{
    public int Number { get; set; }
    public List<Guid>? ReservationId { get; set; } = null!;
    public int Seats { get; set; }
}
