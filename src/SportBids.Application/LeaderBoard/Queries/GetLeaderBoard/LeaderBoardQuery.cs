using MediatR;
using SportBids.Application.Common.Models;

namespace SportBids.Application.LeaderBoard.Queries.GetLeaderBoard;

public class LeaderBoardQuery : IRequest<LeaderBoardModel>
{
    public Guid TournamentId { get; init; }
    public Guid LeaderBoardId { get; init; }
}
