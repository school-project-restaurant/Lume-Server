using MediatR;

namespace Lume.Application.Tables.Commands.DeleteTable;

public class DeleteTableCommand(int number) : IRequest
{
    public int Number { get; set; } = number;
}
