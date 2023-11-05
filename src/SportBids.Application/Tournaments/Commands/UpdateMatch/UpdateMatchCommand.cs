using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SportBids.Domain;

namespace SportBids.Application;

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
