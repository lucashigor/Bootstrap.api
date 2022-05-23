namespace Adasit.Bootstrap.Kernel.Extensions;

using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Domain.Repository;
using Adasit.Bootstrap.Infrastructure.Repositories;
using Adasit.Bootstrap.Infrastructure.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DbContextsExtension
{
    public static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("PrincipalDatabase");


        if (!string.IsNullOrEmpty(conn))
        {
            services.AddDbContext<PrincipalContext>(
                options => options.UseNpgsql(conn));

            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        return services;
    }
}