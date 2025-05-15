using Lume.Application.Extensions;
using Lume.Domain.Entities;
using Lume.Extensions;
using Lume.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using Lume.Infrastructure.Persistence.Seeders;
using Lume.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var orchestrator = scope.ServiceProvider.GetRequiredService<ISeederOrchestrator>();
    await orchestrator.SeedAllAsync();
}

app.UseMiddleware<RequestTimeLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Map("/", () => Results.Redirect("/swagger"));
}

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();

public partial class Program { }