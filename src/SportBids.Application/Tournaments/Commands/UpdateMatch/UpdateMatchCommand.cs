using FluentResults;
using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.Tournaments.Commands.UpdateMatch;

public class UpdateMatchCommand : IRequest<Result>
{
    public Guid TournamentId { get; set; }
    public Guid MatchId { get; set; }
    public Guid Team1Id { get; set; }
    public Guid Team2Id { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public bool Finished { get; set; }
    public Score Score { get; set; }
}
