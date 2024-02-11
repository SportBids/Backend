using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.UpdateTeam;

public class UpdateTeamCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
    public Guid TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsKnockedOut { get; set; }
}
