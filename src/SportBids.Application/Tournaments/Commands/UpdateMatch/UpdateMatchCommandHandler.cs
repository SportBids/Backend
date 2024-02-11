using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Domain.Entities;

namespace SportBids.Application.Tournaments.Commands.UpdateMatch;

public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPredictionScoreCalculationService _scoreCalculationService;

    public UpdateMatchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPredictionScoreCalculationService scoreCalculationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _scoreCalculationService = scoreCalculationService;
    }

    public async Task<Result> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await _unitOfWork.Matches.FindAsync(request.TournamentId, request.MatchId, cancellationToken);

        if (match is null)
            return Result.Fail(new MatchNotFoundError(request.MatchId));

        // if (match.Finished)
        //     return Result.Fail(new MatchReadOnlyError(match.Id));

        _mapper.Map(request, match);

        if (match.Finished)
        {
            await UpdatePredictionPointsAsync(match, cancellationToken);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }

    private async Task UpdatePredictionPointsAsync(Match match, CancellationToken cancellationToken)
    {
        var predictions = await _unitOfWork.Predictions.GetPredictionsByMatchIdAsync(match.Id, cancellationToken);
        // if (!predictions.Any())
        //     return;
        foreach (var prediction in predictions)
        {
            prediction.Points = _scoreCalculationService.Calculate(match.Score, prediction.Score);
        }
    }
}
