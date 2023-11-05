using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Commands.CreateGroupMatches;

public class CreateGroupMatchesCommandHandler : IRequestHandler<CreateGroupMatchesCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupMatchesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateGroupMatchesCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _unitOfWork.Tournaments
            .GetTournamentWithGroupsMatchesAndTeamsAsync(request.TournamentId, cancellationToken);

        if (tournament is null)
        {
            return Result.Fail(new TournamentNotFoundError(request.TournamentId));
        }

        if (tournament.IsPublic)
            return Result.Fail("Tournament is public already!");
        if (!tournament.Teams.Any())
            return Result.Fail("Teams not found!");
        if (!tournament.Groups.Any())
            return Result.Fail("Groups not found!");

        foreach (var group in tournament.Groups)
        {
            group.Matches.Clear();
            CreateGroupMatches(group);
        }
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }

    private void CreateGroupMatches(Group group)
    {
        var newMatches = group.Teams.Zip(
            second: group.Teams.Skip(1),
            resultSelector: CreateMatch);

        foreach (var match in newMatches)
        {
            group.Matches.Add(match);
        }
    }

    private Match CreateMatch(Team f, Team s)
    {
        return new Match { Team1 = f, Team2 = s, StartAt = DateTimeOffset.Now };
    }
}
