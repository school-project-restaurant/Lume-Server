namespace Lume.Application.Dishes.Dtos;

public class DishDto
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Calories { get; set; }
    public List<string> Ingredients { get; set; } = null!;
}
