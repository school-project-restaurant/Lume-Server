using Lume.Domain.Entities;

namespace Lume.Application.Plates.Dtos;

public class PlateDto
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Calories { get; set; }
    public List<string> Ingredients { get; set; } = null!;
}
