namespace Adasit.Bootstrap.Kernel.Extensions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

public static class InfrastructureInitializer
{
    private static readonly Assembly DomainAssembly;
    private static readonly Assembly InfrastructureAssembly;
    private static readonly Assembly ApplicationAssembly;

    static InfrastructureInitializer()
    {
        DomainAssembly = AppDomain.CurrentDomain.Load("Adasit.Bootstrap.Domain");
        ApplicationAssembly = AppDomain.CurrentDomain.Load("Adasit.Bootstrap.Application");
        InfrastructureAssembly = AppDomain.CurrentDomain.Load("Adasit.Bootstrap.Infrastructure");
    }

    public static IServiceCollection ConfigureApplicationInfrastructure(this IServiceCollection services)
    {
        var assembly1 = Assembly.GetExecutingAssembly();

        services.AddMediatR(assembly1,
            DomainAssembly,
            InfrastructureAssembly, 
            ApplicationAssembly);

        return services;
    }
}
