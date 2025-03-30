using System.Text.Json;
using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class TableSeeder(RestaurantDbContext dbContext) : BaseSeeder, ITableSeeder
{
    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

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