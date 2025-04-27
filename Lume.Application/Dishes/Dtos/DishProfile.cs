using AutoMapper;
using Lume.Application.Dishes.Commands.CreateDish;
using Lume.Application.Dishes.Commands.UpdateDish;
using Lume.Domain.Entities;

namespace Lume.Application.Dishes.Dtos;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<UpdateDishCommand, Dish>();
    }
}
