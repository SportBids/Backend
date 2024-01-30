using SportBids.Application.Common.Models;
using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface IPredictionRepository : IRepositoryBase<Prediction, Guid>
{
    Task<IEnumerable<Prediction>> GetOwnPredictionsByMatchIdAsync(IEnumerable<Guid> matchIds, Guid ownerId, CancellationToken cancellationToken);
    Task<IEnumerable<MatchWithPredictionModel>> GetMatchesWithPredictionAsync(Guid ownerId, CancellationToken cancellationToken);
    Task<IEnumerable<Prediction>> GetPredictionsByMatchIdAsync(Guid matchId, CancellationToken cancellationToken);
}
