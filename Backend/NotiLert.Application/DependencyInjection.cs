using Microsoft.Extensions.DependencyInjection;

namespace NotiLert.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Future application services, validators, MediatR handlers will be registered here
        return services;
    }
}
