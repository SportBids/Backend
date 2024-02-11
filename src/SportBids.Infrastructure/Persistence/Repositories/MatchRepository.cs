using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class MatchRepository : RepositoryBase<Match, Guid>, IMatchRepository
{
    public MatchRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Match?> FindAsync(Guid tournamentId, Guid matchId, CancellationToken cancellationToken)
    {
        var match = await _context.Set<Tournament>()
            .Where(tournament => tournament.Id == tournamentId)
            .SelectMany(tournament => tournament.KnockOutMatches)
            .Where(match => match.Id == matchId)
            .Union(_context.Set<Tournament>()
                .Where(tournament => tournament.Id == tournamentId)
                .SelectMany(tournament => tournament.Groups)
                .SelectMany(group => group.Matches)
                .Where(match => match.Id == matchId))
            .SingleOrDefaultAsync(cancellationToken);
        return match;
    }

    public async Task<Match?> GetWithPredictionsAsync(Guid tournamentId, Guid matchId, CancellationToken cancellationToken)
    {
        var match = await _context.Set<Tournament>()
            .Where(tournament => tournament.Id == tournamentId)
            .SelectMany(tournament => tournament.KnockOutMatches)
            .Where(match => match.Id == matchId)
            .Include(match => match.Predictions)
            .SingleOrDefaultAsync(cancellationToken);
        return match;
    }
}
