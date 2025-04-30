using Lume.Application.Tables.Dtos;
using MediatR;

namespace Lume.Application.Tables.Queries.GetAllTables;

public class GetAllTableQuery : IRequest<IEnumerable<TablesDto>>
{

}
