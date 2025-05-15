using AutoMapper;
using Lume.Application.Dishes.Commands.CreateDish;
using Lume.Application.Dishes.Commands.UpdateDish;
using Lume.Application.Dishes.Queries.GetAllDishes;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Application.Dishes.Dtos;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<UpdateDishCommand, Dish>();
        CreateMap<GetAllDishesQuery, DishFilterOptions>();
        CreateMap<GetAllDishesQuery, DishSortOptions>();
    }
}
