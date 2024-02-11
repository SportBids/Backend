using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.Tournaments.Commands.DeleteKnockoutMatch;

public class DeleteKnockOutMatchCommandHandler : IRequestHandler<DeleteKnockOutMatchCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteKnockOutMatchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteKnockOutMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await _unitOfWork.Matches.GetWithPredictionsAsync(request.TournamentId, request.MatchId, cancellationToken);
        if (match is null)
            return Result.Fail(new MatchNotFoundError(request.MatchId));

        if (match.Finished)
            return Result.Fail(new MatchReadOnlyError(match.Id));

        _unitOfWork.Matches.Delete(match);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
