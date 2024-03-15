using FluentResults;
using MediatR;

namespace SportBids.Application.LeaderBoard.Commands.Join;

public class JoinPrivateLeaderBoardCommand : IRequest<Result>
{
    public JoinPrivateLeaderBoardCommand()
    {
    }
    public Guid UserId { get; init; }
    public string JoinCode { get; init; }
}
