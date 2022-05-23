namespace Adasit.Bootstrap.Kernel.Extensions;

using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Infrastructure.Services.FeatureFlag;
using Microsoft.Extensions.DependencyInjection;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IFeatureFlagService, FeatureFlagService>();

        return services;
    }
}