using Lume.Domain.Entities;

using AutoMapper;
using Lume.Application.Plates.Commands.CreatePlate;
using Lume.Application.Plates.Commands.UpdatePlate;

namespace Lume.Application.Plates.Dtos;

public class PlateProfile : Profile
{
    public PlateProfile()
    {
        CreateMap<Plate, PlateDto>();
        CreateMap<CreatePlateCommand, Plate>();
        CreateMap<UpdatePlateCommand, Plate>();
    }
}
