namespace Lume.Infrastructure.Persistence.Seeders;

public interface ISeederOrchestrator
{
    Task SeedAllAsync();
}