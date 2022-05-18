namespace Adasit.Bootstrap.Kernel.Extensions;
using Adasit.Bootstrap.Application.Models;
using Adasit.Bootstrap.Application.UseCases.Configurations.Commands;
using Microsoft.Extensions.DependencyInjection;
public static class UseCasesExtension
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<Notifier>();

        services.AddTransient<IDateValidationHandler, DateValidationHandler>();

        return services;
    }
}