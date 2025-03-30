using System.Text.Json;
using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class TableSeeder(RestaurantDbContext dbContext) : ITableSeeder
{
    private static readonly string SeedDataPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "utility", "seeders.json");
    
    public async Task SeedAsync()
    {
        var absolutePath = Path.GetFullPath(SeedDataPath);
        if (!File.Exists(absolutePath))
        {
            throw new FileNotFoundException($"File not found: {absolutePath}. " +
                                            $"Make sure to create 'utility/seeders.json' in Lume project root.");
        }

        var jsonContent = await File.ReadAllTextAsync(absolutePath);
        var seedData = JsonSerializer.Deserialize<SeedData>(jsonContent, new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true })!;

        if (await dbContext.Database.CanConnectAsync() && !dbContext.Tables.Any())
        {
            foreach (var tableData in seedData.Tables)
            {
                var table = new Table
                {
                    Number = tableData.Number,
                    ReservationsId = tableData.ReservationsId,
                    Seats = tableData.Seats
                };

                dbContext.Tables.Add(table);
            }

            await dbContext.SaveChangesAsync();
        }
            
    }
    
    private class SeedData
    {
        public IEnumerable<TableSeedDataModel> Tables { get; set; } = [];
    }

    private class TableSeedDataModel
    {
        public int Number { get; set; }
        public IEnumerable<string> ReservationsId { get; set; } = [];
        public int Seats { get; set; }
    }
}