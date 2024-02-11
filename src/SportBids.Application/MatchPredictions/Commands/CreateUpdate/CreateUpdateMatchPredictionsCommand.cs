#nullable disable

using FluentResults;
using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.MatchPredictions.Commands.CreateUpdate;

public class CreateUpdateMatchPredictionsCommand : IRequest<Result>
{
    public Prediction[] Predictions { get; set; }
    public Guid OwnerId { get; set; }
    public Guid CreatedById { get; set; }
}
