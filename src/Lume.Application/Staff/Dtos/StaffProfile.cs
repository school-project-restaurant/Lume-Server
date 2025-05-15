using AutoMapper;
using Lume.Application.Staff.Commands.CreateStaff;
using Lume.Application.Staff.Commands.UpdateStaff;
using Lume.Application.Staff.Queries.GetAllStaff;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Application.Staff.Dtos;

public class StaffProfile : Profile
{
    public StaffProfile()
    {
        CreateMap<ApplicationUser, StaffDto>();
        CreateMap<CreateStaffCommand, ApplicationUser>();
        CreateMap<UpdateStaffCommand, ApplicationUser>();
        CreateMap<GetAllStaffQuery, StaffFilterOptions>();
        CreateMap<GetAllStaffQuery, StaffSortOptions>();
    }
}