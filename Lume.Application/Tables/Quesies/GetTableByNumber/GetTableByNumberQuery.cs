using Lume.Application.Tables.Dtos;
using MediatR;

namespace Lume.Application.Customers.Queries.GetTableByNumber;

public class GetTableByNumberQuery(int number) : IRequest<TablesDto?>
{
    public int Id { get; set; } = number;
}
