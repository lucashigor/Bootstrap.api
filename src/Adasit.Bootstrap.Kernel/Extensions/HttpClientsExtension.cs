namespace Adasit.Bootstrap.Kernel.Extensions;

using Adasit.Bootstrap.Application.Models.Config;
using Adasit.Bootstrap.Infrastructure.Services.FeatureFlag;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Refit;
using System;

public static class HttpClientsExtension
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var url = new Uri(configuration["url-gateway"]);

        string subscriptionKey = configuration["InternalSubscriptionKey"];

        string featureFlagBaseUrl = configuration["FeatureFlagIntegration:Url"];

        services
            .AddRefitClient<IFeatureFlagClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(url, featureFlagBaseUrl))
            .ConfigureHttpClient(c => c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey))
            .AddPolicyHandler(GetRetryPolicy(configuration))
            .AddPolicyHandler(GetCircuitBreakerPolicy(configuration));

        return services;
    }

    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
    {
        var wrc = new PollyConfigs(
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.Repetitions)}"], 
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.TimeCircuitBreak)}"], 
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.TimeOut)}"]
            );

        Random jitterer = new ();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(int.Parse(wrc.Repetitions),
                _ => TimeSpan.FromMilliseconds(int.Parse(wrc.TimeCircuitBreak)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IConfiguration configuration)
    {
        var wrc = new PollyConfigs(
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.Repetitions)}"],
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.TimeCircuitBreak)}"],
            configuration[$"{PollyConfigs.PollyConfig}:{nameof(PollyConfigs.TimeOut)}"]
            );

        return Policy
            .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(int.Parse(wrc.TimeOut)));
    }
}
