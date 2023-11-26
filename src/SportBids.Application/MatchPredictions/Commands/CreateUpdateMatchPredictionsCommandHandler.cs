using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application;

public class CreateUpdateMatchPredictionsCommandHandler : IRequestHandler<CreateUpdateMatchPredictionsCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateUpdateMatchPredictionsCommandHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        CreateUpdateMatchPredictionsCommand command,
        CancellationToken cancellationToken)
    {
        var matchIds = command
            .Predictions
            .Select(p => p.MatchId);
        if (await HasFinishedMatches(matchIds, cancellationToken))
        {
            return new PredictionOnFinishedMatchError();
        }

        var savedPredictions = await _unitOfWork
            .Predictions
            .GetOwnPredictionsByMatchId(matchIds, command.OwnerId, cancellationToken);
        var savedPredictionsDict = savedPredictions.ToDictionary(item => item.MatchId, item => item);

        var now = _dateTimeProvider.UtcNow;
        foreach (var newPrediction in command.Predictions)
        {
            if (savedPredictionsDict.ContainsKey(newPrediction.MatchId))
            {
                savedPredictionsDict[newPrediction.MatchId].Score = newPrediction.Score;
                savedPredictionsDict[newPrediction.MatchId].CreatedById = command.CreatedById;
                savedPredictionsDict[newPrediction.MatchId].ModifiedAt = now;
            }
            else
            {
                newPrediction.CreatedAt = now;
                newPrediction.ModifiedAt = now;
                newPrediction.OwnerId = command.OwnerId;
                newPrediction.CreatedById = command.CreatedById;
                newPrediction.Points = 0;
                newPrediction.Score = newPrediction.Score;
                _unitOfWork.Predictions.Add(newPrediction);
            }
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }

    private async Task<bool> HasFinishedMatches(IEnumerable<Guid> matchIds, CancellationToken cancellationToken)
    {
        var c = await _unitOfWork.Matches.GetByIdAsync(matchIds.ToArray(), cancellationToken);
        return c.Any(match => match.Finished);
    }
}
