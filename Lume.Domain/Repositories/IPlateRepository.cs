using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

/// <summary>
/// Public repository for managing prenotations
/// </summary>
public interface IPlateRepository
{

    Task<IEnumerable<Plate>> GetAllPlates();
    Task<Plate?> GetPlateById(Guid id);
    Task<Guid> CreatePlate(Plate plate);
    Task DeletePlate(Plate plate);
    Task SaveChanges();
}
