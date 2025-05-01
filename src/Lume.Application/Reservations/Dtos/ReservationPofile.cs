using Lume.Domain.Entities;

using AutoMapper;
using Lume.Application.Reservations.Commands.CreateReservation;
using Lume.Application.Reservations.Commands.UpdateReservation;

namespace Lume.Application.Reservations.Dtos;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>();
        CreateMap<CreateReservationCommand, Reservation>();
        CreateMap<UpdateReservationCommand, Reservation>();
    }
}
