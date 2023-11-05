using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class GroupRepository : RepositoryBase<Group, Guid>, IGroupRepository
{
    public GroupRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Group?> GetGroupWithTeamsAndTournamentAsync(Guid tournamentId, Guid groupId, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Group>()
            .Where(group => group.Id == groupId && group.Tournament.Id == tournamentId)
            .Include(group => group.Tournament)
            .Include(group => group.Teams)
            .SingleOrDefaultAsync(cancellationToken);
        return entity;
    }
}
