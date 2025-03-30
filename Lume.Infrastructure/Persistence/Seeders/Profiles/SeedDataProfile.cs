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
                    : src.Date.ToUniversalTime()));

        CreateMap<TableSeedDataModel, Table>();

        CreateMap<CustomerSeedDataModel, Customer>()
            .ForMember(dest => dest.UserName, opt => 
                opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserType, opt => 
                opt.MapFrom(_ => "Customer"));

        CreateMap<StaffSeedDataModel, Staff>()
            .ForMember(dest => dest.UserName, opt => 
                opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserType, opt => 
                opt.MapFrom(_ => "Staff"));
    }
}