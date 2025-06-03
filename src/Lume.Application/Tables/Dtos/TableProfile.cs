using AutoMapper;
using Lume.Application.Tables.Commands.CreateTable;
using Lume.Application.Tables.Commands.UpdateTable;
using Lume.Application.Tables.Queries.GetAllTables;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Application.Tables.Dtos;

public class TablesProfile : Profile
{
    public TablesProfile()
    {
        // Entity to DTO mappings
        CreateMap<Table, TablesDto>();
        
        // Command to entity mappings
        CreateMap<CreateTableCommand, Table>();
        CreateMap<UpdateTableCommand, Table>();
        
        // Query to filter/sort options mappings
        CreateMap<GetAllTableQuery, TableFilterOptions>();
        CreateMap<GetAllTableQuery, TableSortOptions>();
    }
}
