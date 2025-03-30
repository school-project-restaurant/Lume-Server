using Lume.Application.Extensions;
using Lume.Infrastructure.Extensions;
using Lume.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userSeeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
    var tableSeeder = scope.ServiceProvider.GetRequiredService<ITableSeeder>();
    var reservationSeeder = scope.ServiceProvider.GetRequiredService<IReservationSeeder>();

    await userSeeder.SeedAsync();
    await tableSeeder.SeedAsync();
    await reservationSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();