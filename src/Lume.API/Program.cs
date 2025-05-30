using Lume.Application.Extensions;
using Lume.Domain.Entities;
using Lume.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using Lume.Infrastructure.Persistence.Seeders;
using Lume.Middlewares;
using Serilog;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication();

// here is Graphana part

// add prometheus exporter
// Replace the current OpenTelemetry configuration with this:
builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {
        var metricsBuilder = opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Lume.API"))
            .AddMeter(builder.Configuration.GetValue<string>("LumeMeter") ?? "Lume.Metrics")
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddPrometheusExporter();

        // Only add OTLP exporter if endpoint is configured
        var otlpEndpoint = builder.Configuration["Otel:Endpoint"];
        if (!string.IsNullOrEmpty(otlpEndpoint))
        {
            metricsBuilder.AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(otlpEndpoint);
            });
        }
    });

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            []
        }
    });
});

builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<RequestTimeLoggingOptions>(options =>
{
    options.MaxBodyLogSize = 500;
    options.LogSuccessResponseBody = false;
    options.LogErrorResponseBody = true;
    options.ExcludePaths = ["/swagger"];
});
builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var orchestrator = scope.ServiceProvider.GetRequiredService<ISeederOrchestrator>();
    await orchestrator.SeedAllAsync();
}

app.UseMiddleware<RequestTimeLoggingMiddleware>();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseSerilogRequestLogging();

/*if (app.Environment.IsDevelopment())
{
}*/
// TODO add a check for production
app.UseSwagger();
app.UseSwaggerUI();

app.Map("/", () => Results.Redirect("/swagger"));

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();

public partial class Program
{
}
