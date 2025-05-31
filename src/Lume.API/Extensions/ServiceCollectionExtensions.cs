using Lume.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Lume.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        
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

        builder.Services.Configure<RequestTimeLoggingOptions>(options => {
            options.MaxBodyLogSize = 500;
            options.LogSuccessResponseBody = false;
            options.LogErrorResponseBody = true;
            options.ExcludePaths = ["/swagger"];
        });
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        builder.Services.AddScoped<ExceptionHandlerMiddleware>();
    }
}