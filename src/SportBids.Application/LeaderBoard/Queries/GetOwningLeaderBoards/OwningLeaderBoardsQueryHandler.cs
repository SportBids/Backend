using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Queries.GetOwningLeaderBoards;

public class OwningLeaderBoardsQueryHandler : IRequestHandler<OwningLeaderBoardsQuery, IEnumerable<PrivateLeaderBoard>>
{
    private readonly IUnitOfWork _unitOfWork;

    public OwningLeaderBoardsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PrivateLeaderBoard>> Handle(
        OwningLeaderBoardsQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _unitOfWork
                         .PrivateLeaderBoards
                         .GetByOwnerIdAsync(request.OwnerId, cancellationToken);
        return list;
    }
}
