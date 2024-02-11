using System.Security.Claims;
using SportBids.Api.Extensions;
using SportBids.Application;
using SportBids.Domain.Entities;
using SportBids.Infrastructure;

namespace SportBids.Api;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(builder.Configuration);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        builder.Services.AddAuthorization(
            configure =>
            {
                configure.AddPolicy(
                    "adminOnly",
                    policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.Administrator.ToString()));
                configure.AddPolicy(
                    "adminOrModerator",
                    policy => policy.RequireClaim(
                        claimType: ClaimTypes.Role,
                        allowedValues: new[] { UserRoles.Administrator.ToString(), UserRoles.Moderator.ToString() }));
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
            app.SeedData();
        }

        app.UseCors(
            config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();
            });
        // app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
