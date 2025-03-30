namespace Lume.Infrastructure.Persistence.Seeders.TableSeeders;

public interface ITableSeeder
{
    Task SeedAsync();
}