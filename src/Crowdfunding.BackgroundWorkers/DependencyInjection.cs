using Microsoft.Extensions.DependencyInjection;

namespace Crowdfunding.BackgroundWorkers;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundWorkers(this IServiceCollection services)
    {
        services.AddHostedService<ProjectOnboardingConsumer>();
        return services;
    }
}
