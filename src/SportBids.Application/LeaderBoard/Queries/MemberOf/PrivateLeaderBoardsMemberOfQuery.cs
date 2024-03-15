using MediatR;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Queries.MemberOf;

public class PrivateLeaderBoardsMemberOfQuery : IRequest<IEnumerable<PrivateLeaderBoard>>
{
    public Guid UserId { get; init; }
}
