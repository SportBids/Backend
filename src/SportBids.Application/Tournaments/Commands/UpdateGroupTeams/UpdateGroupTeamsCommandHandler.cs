using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Commands.UpdateGroupTeams;

public class UpdateGroupTeamsCommandHandler : IRequestHandler<UpdateGroupTeamsCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGroupTeamsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateGroupTeamsCommand command, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork
            .Groups
            .GetGroupWithTeamsAndTournamentAsync(command.TournamentId, command.GroupId, cancellationToken);
        if (group is null)
            return Result.Fail(new GroupNotFoundError(command.GroupId));

        if (group.Tournament.IsPublic)
            return Result.Fail(new TournamentReadOnlyError(command.TournamentId));

        var teams = await _unitOfWork
            .Teams
            .GetTeamsAsync(command.TournamentId, command.TeamIds, cancellationToken);

        var notFoundTeamIds = command.TeamIds.Where(teamId => !teams.Any(team => team.Id == teamId)).ToArray();
        if (notFoundTeamIds.Any())
            return Result.Fail(notFoundTeamIds.Select(id => new TeamNotFoundError(id)));

        group.Teams.Clear();
        foreach (var team in teams)
        {
            group.Teams.Add(team);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
