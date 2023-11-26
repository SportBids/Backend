#nullable disable

using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application;

public class CreateUpdateMatchPredictionsCommand : IRequest<Result>
{
    public Prediction[] Predictions { get; set; }
    public Guid OwnerId { get; set; }
    public Guid CreatedById { get; set; }
}
