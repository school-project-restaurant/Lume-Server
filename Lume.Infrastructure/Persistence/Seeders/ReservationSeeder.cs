using System.Text.Json;
using System.Text.Json.Serialization;
using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class ReservationSeeder(RestaurantDbContext dbContext) : IReservationSeeder
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
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new StatusEnumConverter() }
        };
        var seedData = JsonSerializer.Deserialize<SeedData>(jsonContent, options)!;

        if (await dbContext.Database.CanConnectAsync() && !dbContext.Reservations.Any())
        {
            foreach (var reservationData in seedData.Reservations)
            {
                var reservation = new Reservation
                {
                    Id = reservationData.Id,
                    CustomerId = reservationData.CustomerId,
                    Date = reservationData.Date.Kind == DateTimeKind.Unspecified 
                        ? DateTime.SpecifyKind(reservationData.Date, DateTimeKind.Utc)
                        : reservationData.Date.ToUniversalTime(),
                    TableNumber = reservationData.TableNumber,
                    GuestCount = reservationData.GuestCount,
                    Status = reservationData.Status,
                    Notes = reservationData.Notes
                };

                dbContext.Reservations.Add(reservation);
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private class StatusEnumConverter : JsonConverter<Status>
    {
        public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected string value for Status enum, but got {reader.TokenType}");
            }

            var enumString = reader.GetString();
            if (Enum.TryParse<Status>(enumString, true, out var status))
            {
                return status;
            }

            throw new JsonException($"Invalid value '{enumString}' for Status enum");
        }

        public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private class SeedData
    {
        public IEnumerable<ReservationSeedDataModel> Reservations { get; set; } = [];
    }

    private class ReservationSeedDataModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public int TableNumber { get; set; }
        public int GuestCount { get; set; }
        public Status Status { get; set; }
        public string? Notes { get; set; }
    }
}