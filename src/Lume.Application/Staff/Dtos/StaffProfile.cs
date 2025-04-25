using AutoMapper;
using Lume.Application.Staff.Commands.CreateStaff;
using Lume.Application.Staff.Commands.UpdateStaff;
using Lume.Domain.Entities;

namespace Lume.Application.Staff.Dtos;

public class StaffProfile : Profile
{
    public StaffProfile()
    {
        CreateMap<ApplicationUser, StaffDto>();
        CreateMap<CreateStaffCommand, ApplicationUser>();
        CreateMap<UpdateStaffCommand, ApplicationUser>();
    }
}