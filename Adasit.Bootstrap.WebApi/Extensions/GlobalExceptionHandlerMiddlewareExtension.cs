namespace Adasit.Bootstrap.WebApi.Extensions;

using Adasit.Bootstrap.WebApi.Middlewares;

public static class GlobalExceptionHandlerMiddlewareExtension
{
    public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
    {
        return services.AddTransient<GlobalExceptionHandlerMiddleware>();
    }

    public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
