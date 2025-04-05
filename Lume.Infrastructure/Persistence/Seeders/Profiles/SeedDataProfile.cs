using AutoMapper;
using Lume.Domain.Entities;
using Lume.Infrastructure.Persistence.Seeders.Models;

namespace Lume.Infrastructure.Persistence.Seeders.Profiles;

public class SeedDataProfile : Profile
{
    public SeedDataProfile()
    {
        CreateMap<ReservationSeedDataModel, Reservation>()
            .ForMember(dest => dest.Date, opt => 
                opt.MapFrom(src => 
                src.Date.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)
                    : src.Date.ToUniversalTime()))
            .ForMember(dest => dest.Status, opt => 
                opt.MapFrom(src => 
                string.IsNullOrEmpty(src.Status) 
                    ? "Pending" 
                    : char.ToUpper(src.Status[0]) + src.Status.Substring(1)));;

        CreateMap<TableSeedDataModel, Table>();

        CreateMap<CustomerSeedDataModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => 
                opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserType, opt => 
                opt.MapFrom(_ => "Customer"));

        CreateMap<StaffSeedDataModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => 
                opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserType, opt => 
                opt.MapFrom(_ => "Staff"));
    }
}