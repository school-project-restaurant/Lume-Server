namespace Lume.Domain.Entities;

public class Plate
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Calories { get; set; }
    public List<string> Ingredients { get; set; } = null!; // "new List<string>();"?
}
// ## IMPORTANT ##
// might add something that could help pepole
// with allergies that if you enter an ingredients
// it tell you what you could eat with no risk
// such as a predeterminated list of obejcts with
// ingredients, or just a common string
