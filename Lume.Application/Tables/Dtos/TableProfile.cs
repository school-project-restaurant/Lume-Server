using AutoMapper;
using Lume.Application.Tables.Commands.CreateTable;
using Lume.Application.Tables.Commands.UpdateTable;
using Lume.Application.Tables.Commands.DeleteTable;
using Lume.Domain.Entities;

namespace Lume.Application.Tables.Dtos;

public class TablesProfile : Profile
{
    public TablesProfile()
    {
        CreateMap<ApplicationUser, TablesDto>();
        CreateMap<CreateTableCommand, ApplicationUser>();
        CreateMap<UpdateTableCommand, ApplicationUser>();
        CreateMap<DeleteTableCommand, ApplicationUser>();
    }
}
