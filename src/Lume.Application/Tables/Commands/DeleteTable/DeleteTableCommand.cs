using MediatR;

namespace Lume.Application.Tables.Commands.DeleteTable;

public class DeleteTableCommand(int number) : IRequest<bool>
{
    public int Number { get; set; } = number;
}
