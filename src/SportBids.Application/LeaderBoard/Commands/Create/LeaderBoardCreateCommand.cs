using FluentResults;
using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Commands.Create;

public class LeaderBoardCreateCommand : IRequest<Result<PrivateLeaderBoard>>
{
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
}
