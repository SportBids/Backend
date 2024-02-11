using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class TeamRepository : RepositoryBase<Team, Guid>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Team?> GetTeamAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken)
    {
        var team = await _context.Set<Tournament>()
            .Where(tournament => tournament.Id == tournamentId)
            .SelectMany(tournament => tournament.Teams)
            .Where(team => team.Id == teamId)
            .SingleOrDefaultAsync(cancellationToken);
        return team;
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync(Guid tournamentId, IEnumerable<Guid> teamIds, CancellationToken cancellationToken)
    {
        // var teams = await _context.Set<Team>()
        //     .Where(team => team.Tournament.Id == tournamentId && teamIds.Contains(team.Id))
        //     .ToListAsync(cancellationToken);
        var teams = await _context.Set<Tournament>()
            .Where(tournament => tournament.Id == tournamentId)
            .SelectMany(tournament => tournament.Teams)
            .Where(team => teamIds.Contains(team.Id))
            .ToListAsync(cancellationToken);
        return teams;
    }
}
