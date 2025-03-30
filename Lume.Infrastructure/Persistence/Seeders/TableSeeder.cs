using System.Text.Json;
using AutoMapper;
using Lume.Domain.Entities;
using Lume.Infrastructure.Persistence.Seeders.Models;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class TableSeeder(RestaurantDbContext dbContext,
    IMapper mapper) : BaseSeeder, ITableSeeder
{
    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

        if (await dbContext.Database.CanConnectAsync() && !dbContext.Tables.Any())
        {
            foreach (var tableData in seedData.Tables)
            {
                var table = mapper.Map<Table>(tableData);

                dbContext.Tables.Add(table);
            }

            await dbContext.SaveChangesAsync();
        }
            
    }
}