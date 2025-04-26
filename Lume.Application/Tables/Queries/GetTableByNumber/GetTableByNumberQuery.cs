using Lume.Application.Tables.Dtos;
using MediatR;

namespace Lume.Application.Tables.Queries.GetTableByNumber;

public class GetTableByNumberQuery(int number) : IRequest<TablesDto?>
{
    public int Number { get; set; } = number;
}
