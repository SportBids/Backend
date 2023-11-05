using FluentResults;
using FluentValidation;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.UpdateGroupTeams;

/// <summary>
/// Assigns teams to group
/// </summary>
public class UpdateGroupTeamsCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
    public Guid GroupId { get; set; }
    public List<Guid> TeamIds { get; set; } = null!;
}
