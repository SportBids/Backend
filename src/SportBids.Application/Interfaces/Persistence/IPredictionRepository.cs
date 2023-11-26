using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface IPredictionRepository : IRepositoryBase<Prediction, Guid>
{
    Task<IEnumerable<Prediction>> GetOwnPredictionsByMatchId(IEnumerable<Guid> matchIds, Guid ownerId, CancellationToken cancellationToken);
}
