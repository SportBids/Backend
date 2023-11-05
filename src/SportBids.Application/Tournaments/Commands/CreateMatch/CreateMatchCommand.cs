using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.CreateMatch;

/// <summary>
/// Creates knock-out match in tournament
/// </summary>
public class CreateMatchCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
    public Guid Team1Id { get; set; }
    public Guid Team2Id { get; set; }
    public DateTimeOffset StartAt { get; set; }
}
