using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Persistence;

public interface IGroupRepository : IRepositoryBase<Group, Guid>
{
    Task<Group?> GetGroupWithTeamsAndTournamentAsync(Guid tournamentId, Guid groupId, CancellationToken cancellationToken);
}
