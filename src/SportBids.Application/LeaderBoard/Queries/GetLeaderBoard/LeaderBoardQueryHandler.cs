using MediatR;
using SportBids.Application.Common.Models;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.LeaderBoard.Queries.GetLeaderBoard;

public class LeaderBoardQueryHandler : IRequestHandler<LeaderBoardQuery, LeaderBoardModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaderBoardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<LeaderBoardModel> Handle(LeaderBoardQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LeaderBoardItemModel> response;
        if (request.LeaderBoardId.Equals(default))
        {
            response = await _unitOfWork
                             .PrivateLeaderBoards
                             .GetGlobalLeaderBoardAsync(request.TournamentId, cancellationToken);
        }
        else
        {
            response = await _unitOfWork
                             .PrivateLeaderBoards
                             .GetPrivateLeaderBoardAsync(request.LeaderBoardId, request.TournamentId, cancellationToken);    
        }

        var leaderBoard = new LeaderBoardModel { Items = response };
        return leaderBoard;
    }
}
