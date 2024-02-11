using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using SportBids.Domain;

class DBSeeder : IDBInitializer
{
    public async Task Initialize(IServiceProvider serviceProvider)
    {
        await SeedUser(serviceProvider);
    }
    private async Task SeedUser(IServiceProvider serviceProvider)
    {
        var authService = serviceProvider.GetRequiredService<IAuthService>();

        int totalUsersCount = await authService.GetUsersCountAsync();
        if (totalUsersCount > 0) return;

        var user = new AppUser() { UserName = "administrator", Email = "no@mail.ru" };
        var pass = RandomPassword(20);

        var result = await authService.Create(user, pass);
        if (result.IsFailed)
            throw new Exception($"Failed to create 'administrator' while initial seeding: ${result.Errors.Select(x => x.Message).Aggregate("", (a, b) => string.Join(" ", a, b))}");

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Role, UserRoles.Administrator.ToString())
        };
        var claimAdditionResult = await authService.AddUserClaimsAsync(result.Value, claims);
        if (claimAdditionResult.IsFailed)
            throw new Exception(claimAdditionResult.Errors.First().Message);

        Console.WriteLine($"Initial user created with username='{user.UserName}' and password '{pass}'");
    }

    private string RandomPassword(int length)
    {
        byte[] rgb = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(rgb);
    }
}
