using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using AutoMapper;
using Lume.Application.Reservations.Commands.CreateReservation;
using Lume.Application.Reservations.Commands.UpdateReservation;
using Lume.Application.Reservations.Queries.GetAllReservations;

namespace Lume.Application.Reservations.Dtos;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        // Entity to DTO mappings
        CreateMap<Reservation, ReservationDto>();
        
        // Command to entity mappings
        CreateMap<CreateReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
        
        // Query to filter/sort options mappings
        CreateMap<GetAllReservationsQuery, ReservationFilterOptions>();
        CreateMap<GetAllReservationsQuery, ReservationSortOptions>();
    }
}
