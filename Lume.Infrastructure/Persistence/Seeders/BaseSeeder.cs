using System.Text.Json;
using System.Text.Json.Serialization;
using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders;

internal abstract class BaseSeeder
{
    private static readonly string SeedDataPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "utility", "seeders.json");
    
    protected async Task<T> LoadSeedDataAsync<T>(JsonSerializerOptions? options = null) where T : class, new()
    {
        var absolutePath = Path.GetFullPath(SeedDataPath);
        if (!File.Exists(absolutePath))
        {
            throw new FileNotFoundException($"File not found: {absolutePath}. " +
                                            $"Make sure to create 'utility/seeders.json' in Lume project root.");
        }
    
        var jsonContent = await File.ReadAllTextAsync(absolutePath);
        options ??= new JsonSerializerOptions { 
            PropertyNameCaseInsensitive = true,
            Converters = { new StatusEnumConverter() }
        };
        
        return JsonSerializer.Deserialize<T>(jsonContent, options) ?? new T();
    }

    private class StatusEnumConverter : JsonConverter<Status>
    {
        public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var enumString = reader.GetString();
                if (Enum.TryParse<Status>(enumString, true, out var status))
                {
                    return status;
                }
                throw new JsonException($"Invalid value '{enumString}' for Status enum");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var enumValue = reader.GetInt32();
                if (Enum.IsDefined(typeof(Status), enumValue))
                {
                    return (Status)enumValue;
                }
                throw new JsonException($"Invalid numeric value {enumValue} for Status enum");
            }
            
            throw new JsonException($"Expected string or number value for Status enum, but got {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((int)value);
        }
    }
}