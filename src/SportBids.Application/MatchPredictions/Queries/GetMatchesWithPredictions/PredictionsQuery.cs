using FluentResults;
using MediatR;
using SportBids.Application.Common.Models;

namespace SportBids.Application.MatchPredictions.Queries.GetMatchesWithPredictions;

public class MatchesWithPredictionQuery : IRequest<Result<IEnumerable<MatchWithPredictionModel>>>
{
    public Guid UserId { get; set; }
}
