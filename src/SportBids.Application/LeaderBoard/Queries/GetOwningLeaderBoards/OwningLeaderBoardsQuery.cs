using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Queries.GetOwningLeaderBoards;

public class OwningLeaderBoardsQuery : IRequest<IEnumerable<PrivateLeaderBoard>>
{
    public Guid OwnerId { get; init; }
}
