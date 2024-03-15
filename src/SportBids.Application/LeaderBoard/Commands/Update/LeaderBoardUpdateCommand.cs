using FluentResults;
using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Commands.Update;

public class LeaderBoardUpdateCommand : IRequest<Result<PrivateLeaderBoard>>
{
    public Guid InitiatorId { get; init; }
    public Guid LeaderBoardId { get; init; }
    public string Name { get; init; } = null!;
}
