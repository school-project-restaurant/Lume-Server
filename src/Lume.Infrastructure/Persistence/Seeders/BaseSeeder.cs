using System.Text.Json;

namespace Lume.Infrastructure.Persistence.Seeders;

internal abstract class BaseSeeder
{
    private static readonly string SeedDataPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "utility", "seeders.json");
    
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
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<T>(jsonContent, options) ?? new T();
    }

}