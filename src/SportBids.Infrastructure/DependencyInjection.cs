using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Infrastructure.Authentication;
using SportBids.Infrastructure.Persistence;
using SportBids.Infrastructure.Persistence.Entities;
using SportBids.Infrastructure.Persistence.Repositories;
using SportBids.Infrastructure.Services;

namespace SportBids.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddAuth(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped(typeof(UserManager<AppUser>));
        services.AddScoped<IEmailService, EmailService>();

        // services.AddDbContext<AppDbContext>(
        //     options => options.UseSqlite(
        //         configuration.GetConnectionString("sqlite"), config => config.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));
        services.AddDbContext<AppDbContext>(
            options => options.UseInMemoryDatabase("InMemory"));

        //services
        //    .AddIdentity<AppUser, IdentityRole<Guid>>(
        //        options => options.User.RequireUniqueEmail = true)
        //    .AddEntityFrameworkStores<AppDbContext>()
        //    .AddDefaultTokenProviders();
        services
            .AddIdentityCore<AppUser>(
                options => options.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtFactory, JwtFactory>();

        services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = SecurityKeyProvider.GetSecurityKey(jwtSettings)
                    };
                });
        return services;
    }
}
