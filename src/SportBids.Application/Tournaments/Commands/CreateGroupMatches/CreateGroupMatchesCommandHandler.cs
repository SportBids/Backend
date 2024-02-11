using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

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
            return Result.Fail(new TournamentReadOnlyError(tournament.Id));

        if (tournament.Groups.Count == 0)
            return Result.Fail("Groups not found!");

        if (HasGroupsMissingTeams(tournament, out var errors))
            return Result.Fail(errors);


        foreach (var group in tournament.Groups)
        {
            group.Matches.Clear();
            CreateGroupMatches(group);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }

    private bool HasGroupsMissingTeams(Tournament tournament, out IEnumerable<IError> errors)
    {
        var teamsPerGroup = tournament.Teams.Count / tournament.Groups.Count;
        var groupsMissingTeams = tournament
            .Groups
            .Where(group => group.Teams is null || group.Teams.Count != teamsPerGroup)
            .ToArray();

        if (groupsMissingTeams.Length > 0)
        {
            errors = groupsMissingTeams.Select(group => new GroupTeamsMissingError(group.Name));
            return true;
        }

        errors = Array.Empty<IError>();
        return false;
    }

    private void CreateGroupMatches(Group group)
    {
        var teams = group.Teams.ToArray();
        for (int i = 0; i < teams.Length; i++)
        for (int j = i + 1; j < teams.Length; j++)
        {
            var match = CreateMatch(teams[i], teams[j]);
            group.Matches.Add(match);
        }
    }

    private Match CreateMatch(Team f, Team s)
    {
        return new Match { Team1 = f, Team2 = s, StartAt = DateTimeOffset.Now };
    }
}
