using MediatR;

namespace Lume.Application.Tables.Commands.UpdateTable;

public class UpdateTableCommand : IRequest
{
    public int Number { get; set; }
    public int Seats { get; set; }
}
