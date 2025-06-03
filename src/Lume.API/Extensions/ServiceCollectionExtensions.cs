using System.Threading.RateLimiting;
using Lume.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;

namespace Lume.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
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
                    metricsBuilder.AddOtlpExporter(opts => { opts.Endpoint = new Uri(otlpEndpoint); });
                }
            });
        builder.Services.AddControllers();

        builder.Services.AddAuthentication();

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

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });


        builder.Services.AddEndpointsApiExplorer();

        builder.Services.Configure<RequestTimeLoggingOptions>(options =>
        {
            options.MaxBodyLogSize = 500;
            options.LogSuccessResponseBody = false;
            options.LogErrorResponseBody = true;
            options.ExcludePaths = ["/swagger"];
        });
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        builder.Services.AddScoped<ExceptionHandlerMiddleware>();

        builder.Services.AddMemoryCache();
        
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("FixedPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(30);
                opt.PermitLimit = 2;
                opt.QueueLimit = 0;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
    }
}