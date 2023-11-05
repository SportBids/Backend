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
}
