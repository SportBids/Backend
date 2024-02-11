using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Persistence;

public interface IMatchRepository : IRepositoryBase<Match, Guid>
{
    Task<Match?> FindAsync(Guid tournamentId, Guid matchId, CancellationToken cancellationToken);
    Task<Match?> GetWithPredictionsAsync(Guid tournamentId, Guid matchId, CancellationToken cancellationToken);
}
