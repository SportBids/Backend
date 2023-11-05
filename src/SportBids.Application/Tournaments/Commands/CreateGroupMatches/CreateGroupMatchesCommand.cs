using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.CreateGroupMatches;

/// <summary>
/// Creates matches in a group
/// </summary>
public class CreateGroupMatchesCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
}
