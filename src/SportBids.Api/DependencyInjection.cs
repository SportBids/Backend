using System.Reflection;
using Mapster;
using MapsterMapper;

namespace SportBids.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappings();
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(config =>
            {
                config.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        services.AddScoped<IDBInitializer, DBSeeder>();
        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        // var config = new TypeAdapterConfig();
        // Or
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
