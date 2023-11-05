using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Commands.CreateMatch;

public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMatchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _unitOfWork
            .Tournaments
            .GetTournamentMatchesAndTeamsAsync(request.TournamentId, cancellationToken);

        if (tournament is null)
        {
            return Result.Fail(new TournamentNotFoundError(request.TournamentId));
        }

        var team01 = tournament.Teams.SingleOrDefault(team => team.Id == request.Team1Id);
        if (team01 is null) return Result.Fail(new TeamNotFoundError(request.Team1Id));

        var team02 = tournament.Teams.SingleOrDefault(team => team.Id == request.Team2Id);
        if (team02 is null) return Result.Fail(new TeamNotFoundError(request.Team2Id));

        var newMatch = new Match
        {
            Team1 = team01,
            Team2 = team02,
            StartAt = request.StartAt
        };

        tournament.KnockOutMatches!.Add(newMatch);

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
