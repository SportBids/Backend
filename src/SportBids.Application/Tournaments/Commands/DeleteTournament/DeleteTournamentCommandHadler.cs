using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.Tournaments.Commands.DeleteTournament;

public class DeleteTournamentCommandHadler : IRequestHandler<DeleteTournamentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTournamentCommandHadler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTournamentCommand command, CancellationToken cancellationToken)
    {
        var tournament = await _unitOfWork.Tournaments.GetByIdAsync(command.Id, cancellationToken);
        if (tournament is null)
            return Result.Fail(new TournamentNotFoundError(command.Id));

        if (tournament.IsPublic)
        {
            return Result.Fail(new TournamentReadOnlyError(tournament.Id));
        }
        _unitOfWork.Tournaments.Delete(tournament);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
