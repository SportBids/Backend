using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.DeleteKnockoutMatch;

public class DeleteKnockOutMatchCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
    public Guid MatchId { get; set; }
}
