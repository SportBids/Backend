#nullable disable

using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application.MatchPredictions.Queries.GetMatchesPredictions;

public class MatchPredictionsQuery : IRequest<Result<IEnumerable<Prediction>>>
{
    public Guid[] MatchIds { get; set; }
    public Guid OwnerId { get; set; }
}
