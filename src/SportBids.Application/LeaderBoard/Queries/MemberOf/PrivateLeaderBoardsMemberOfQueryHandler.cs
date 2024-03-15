using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Queries.MemberOf;

public class
    PrivateLeaderBoardsMemberOfQueryHandler : IRequestHandler<PrivateLeaderBoardsMemberOfQuery,
    IEnumerable<PrivateLeaderBoard>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivateLeaderBoardsMemberOfQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<PrivateLeaderBoard>> Handle(PrivateLeaderBoardsMemberOfQuery request, CancellationToken cancellationToken)
    {
        var boards = await _unitOfWork
                           .PrivateLeaderBoards
                           .GetByMemberAsync(request.UserId, cancellationToken);
        return boards;
    }
}
