using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SportBids.Application.Common.Behaviors;
using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Application.MatchPredictions.ScorePointCalculators;
using SportBids.Application.Services;

namespace SportBids.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IPredictionScoreCalculator, CorrectScore>();
        services.AddScoped<IPredictionScoreCalculator, CorrectGoalDifference>();
        services.AddScoped<IPredictionScoreCalculator, CorrectOutcome>();
        services.AddScoped<IPredictionScoreCalculationService, PredictionScoreCalculationService>();

        return services;
    }
}
