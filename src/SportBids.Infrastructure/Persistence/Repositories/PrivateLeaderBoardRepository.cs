using System.Collections;
using Microsoft.EntityFrameworkCore;
using SportBids.Application.Common.Models;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class PrivateLeaderBoardRepository : RepositoryBase<PrivateLeaderBoard, Guid>, IPrivateLeaderBoardRepository
{
    public PrivateLeaderBoardRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PrivateLeaderBoard?> GetByIdAsync(Guid ownerId, Guid boardId, CancellationToken cancellationToken)
    {
        var board = await FindWhere(x => x.Id.Equals(boardId) && x.Owner.Id.Equals(ownerId))
            .SingleOrDefaultAsync(cancellationToken);
        return board;
    }

    public async Task<PrivateLeaderBoard?> GetByIdWithMembersAsync(Guid boardId, CancellationToken cancellationToken)
    {
        var board = await FindWhere(x => x.Id.Equals(boardId))
                          .Include(x => x.Members)
                          .SingleOrDefaultAsync(cancellationToken);
        return board;
    }

    public async Task<IEnumerable<PrivateLeaderBoard>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        var boards = await FindWhere(x => x.Owner.Id.Equals(ownerId))
            .ToListAsync(cancellationToken);
        return boards;
    }

    public async Task<PrivateLeaderBoard?> GetByJoinCodeAsync(string joinCode, CancellationToken cancellationToken)
    {
        var board = await FindWhere(x => x.JoinCode.Equals(joinCode))
            .SingleOrDefaultAsync(cancellationToken);
        return board;
    }

    public async Task<IEnumerable<PrivateLeaderBoard>> GetByMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        return await FindWhere(x => x.Members.Any(u => u.Id.Equals(memberId)))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LeaderBoardItemModel>> GetGlobalLeaderBoardAsync(Guid tournamentId, CancellationToken cancellationToken)
    {
        var leaderBoardItemModel = await GetLeaderBoardQuery(tournamentId)
            .ToArrayAsync(cancellationToken);
        return leaderBoardItemModel;
    }

    public async Task<IEnumerable<LeaderBoardItemModel>> GetPrivateLeaderBoardAsync(Guid leaderBoardId, Guid tournamentId, CancellationToken cancellationToken)
    {
        var leaderBoard = await GetByIdWithMembersAsync(leaderBoardId, cancellationToken);
        if (leaderBoard is null) return Array.Empty<LeaderBoardItemModel>();

        var query = GetLeaderBoardQuery(tournamentId)
            .Where(item => leaderBoard.Members.Contains(item.User));
        var leaderBoardItemModel = await query.ToArrayAsync(cancellationToken);
        return leaderBoardItemModel;
    }

    private IQueryable<LeaderBoardItemModel> GetLeaderBoardQuery(Guid tournamentId)
    {
        return _context
               .Set<Tournament>()
               .Where(
                   tournament => tournament.Id.Equals(tournamentId))
               .SelectMany(
                   tournament => tournament
                                 .KnockOutMatches
                                 .Concat(
                                     tournament
                                         .Groups
                                         .SelectMany(group => group.Matches))
                                 //.Where(withPrediction)
                                 .Join(
                                     _context.Set<Prediction>().AsQueryable(),
                                     match => match.Id,
                                     prediction => prediction.MatchId,
                                     (match, prediction) => prediction)
                                 .GroupBy(
                                     prediction => prediction.Owner,
                                     prediction => prediction.Points,
                                     (user, points) => Tuple.Create(user, points.Sum())
                                 )
                                 .Select(
                                     item => new LeaderBoardItemModel
                                     {
                                         User   = item.Item1,
                                         Points = item.Item2
                                     })
               );
    }
}
