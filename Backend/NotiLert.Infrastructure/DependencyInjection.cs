using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotiLert.Infrastructure.Persistence;

namespace NotiLert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found in configuration.");

        services.AddDbContext<NotiLertDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
