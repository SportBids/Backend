using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application;

public class MatchPredictionsQueryHandler : IRequestHandler<MatchPredictionsQuery, Result<IEnumerable<Prediction>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MatchPredictionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Prediction>>> Handle(MatchPredictionsQuery request, CancellationToken cancellationToken)
    {
        var predictions = await _unitOfWork
            .Predictions
            .GetOwnPredictionsByMatchId(request.MatchIds, request.OwnerId, cancellationToken);
        return Result.Ok(predictions);
    }
}
