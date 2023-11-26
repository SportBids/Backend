using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;
using SportBids.Infrastructure.Persistence;
using SportBids.Infrastructure.Persistence.Repositories;

namespace SportBids.Infrastructure;

public class PredictionRepository : RepositoryBase<Prediction, Guid>, IPredictionRepository
{
    public PredictionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Prediction>> GetOwnPredictionsByMatchId(IEnumerable<Guid> matchIds, Guid ownerId, CancellationToken cancellationToken)
    {
        return await FindWhere(p => p.Owner.Id == ownerId && matchIds.Contains(p.Match.Id))
            .ToListAsync(cancellationToken);
    }
}
