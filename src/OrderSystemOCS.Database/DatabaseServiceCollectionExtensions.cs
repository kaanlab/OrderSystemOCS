using Microsoft.EntityFrameworkCore;
using OrderSystemOCS.Database;
using OrderSystemOCS.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseLayer(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IOrderRepository, NpgSqlRepository>();
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}

