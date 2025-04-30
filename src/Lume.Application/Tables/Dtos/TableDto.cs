namespace Lume.Application.Tables.Dtos;

public class TablesDto
{
    public int Number { get; set; }
    public List<Guid>? reservationsId { get; set; }
    public int Seats { get; set; }
}
