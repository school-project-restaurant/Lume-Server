namespace Lume.Infrastructure.Persistence.Seeders;

internal class SeederOrchestrator(IEnumerable<ISeeder> seeders) : ISeederOrchestrator
{
    public async Task SeedAllAsync()
    {
        foreach (var seeder in seeders)
            await seeder.SeedAsync();
    }
}