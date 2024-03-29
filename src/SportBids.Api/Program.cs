﻿using System.Security.Claims;
using FluentResults;
using SportBids.Api;
using SportBids.Api.Extensions;
using SportBids.Application;
using SportBids.Domain.Entities;
using SportBids.Infrastructure;

internal class Program
{
    private static async Task Main(string[] args)
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

        builder.Services.AddAuthorization(configure =>
        {
            configure.AddPolicy("adminOnly",
                                policy => policy.RequireClaim(ClaimTypes.Role, UserClaims.Administrator.ToString()));
            configure.AddPolicy("adminOrModerator",
                                policy => policy.RequireClaim(ClaimTypes.Role, UserClaims.Administrator.ToString(), UserClaims.Moderator.ToString()));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
            app.SeedData();
        }

        app.UseCors(config =>
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
