using System.Runtime.CompilerServices;

namespace SportBids.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
        seeder.Initialize(scope.ServiceProvider);
    }
}
