namespace Adasit.Bootstrap.Kernel.Extensions;

using Adasit.Bootstrap.Application.Interfaces;
using Adasit.Bootstrap.Infrastructure.rabbitmq;
using Microsoft.Extensions.DependencyInjection;
public static class InfrastructureExtension
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSenderInterface, SendMessageRabbitmq>();

        return services;
    }
}