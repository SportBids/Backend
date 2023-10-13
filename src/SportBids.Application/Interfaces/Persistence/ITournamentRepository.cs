using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface ITournamentRepository : IRepositoryBase<Tournament, Guid>
{
    Task<List<Tournament>> GetAllAsync(CancellationToken cancellationToken);
}
