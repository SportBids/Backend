using System.Collections;
using SportBids.Application.Common.Models;
using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Persistence;

public interface IPrivateLeaderBoardRepository : IRepositoryBase<PrivateLeaderBoard, Guid>
{
    Task<PrivateLeaderBoard?> GetByIdAsync(Guid ownerId, Guid boardId, CancellationToken cancellationToken);
    Task<PrivateLeaderBoard?> GetByIdWithMembersAsync(Guid boardId, CancellationToken cancellationToken);
    Task<IEnumerable<PrivateLeaderBoard>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
    Task<PrivateLeaderBoard?> GetByJoinCodeAsync(string joinCode, CancellationToken cancellationToken);
    Task<IEnumerable<PrivateLeaderBoard>> GetByMemberAsync(Guid memberId, CancellationToken cancellationToken);
    Task<IEnumerable<LeaderBoardItemModel>> GetGlobalLeaderBoardAsync(Guid tournamentId, CancellationToken cancellationToken);
    Task<IEnumerable<LeaderBoardItemModel>> GetPrivateLeaderBoardAsync(Guid leaderBoardId, Guid tournamentId, CancellationToken cancellationToken);
}
