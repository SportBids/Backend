using FluentResults;
using MediatR;
using SportBids.Application.Common.Models;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.MatchPredictions.Queries.GetMatchesWithPredictions;

public class PredictionsQueryHandler : IRequestHandler<MatchesWithPredictionQuery, Result<IEnumerable<MatchWithPredictionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PredictionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<MatchWithPredictionModel>>> Handle(MatchesWithPredictionQuery request, CancellationToken cancellationToken)
    {
        var predictions = await _unitOfWork
            .Predictions
            .GetMatchesWithPredictionAsync(request.UserId, cancellationToken);
        return Result.Ok(predictions);
    }
}
