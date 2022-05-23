namespace Adasit.Bootstrap.WebApi.Extensions;
using Microsoft.OpenApi.Models;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            c.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Title = "AdasIt.Bootstrap.WebApi",
                    Version = "v1",
                    Description = "Api de Example",
                    Contact = new OpenApiContact()
                    {
                        Name = "Api"
                    }
                });
            c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
            {
                Name = "apiKey",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "apiKey"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "apiKey"
                        },
                        Scheme = "apiKey",
                        Name = "apiKey",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                    }
                });
        }).AddSwaggerGenNewtonsoftSupport();

        return services;
    }
}
