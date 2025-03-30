using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Lume.Domain.Entities;
using Lume.Infrastructure.Persistence.Seeders.Models;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class ReservationSeeder(
    RestaurantDbContext dbContext,
    IMapper mapper) : BaseSeeder, IReservationSeeder
{
    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

        if (await dbContext.Database.CanConnectAsync() && !dbContext.Reservations.Any())
        {
            foreach (var reservationData in seedData.Reservations)
            {
                var reservation = mapper.Map<Reservation>(reservationData);
                dbContext.Reservations.Add(reservation);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}