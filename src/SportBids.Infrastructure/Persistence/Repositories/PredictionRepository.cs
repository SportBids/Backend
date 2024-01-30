using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using SportBids.Application.Common.Models;
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

    public async Task<IEnumerable<MatchWithPredictionModel>> GetMatchesWithPredictionAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        /*
            Нужно достать из БД все прогнозы пользователя, соответствующие матчи и турниры
        */

        var t = await _context
            .Set<Tournament>()
            .Where(tournament => tournament
                .KnockOutMatches
                .Any(km => _context.Set<Prediction>()
                    .Where(prediction => prediction.Owner.Id == ownerId)
                    .Select(prediction => prediction.MatchId)
                    .Contains(km.Id))
                || tournament
                    .Groups
                    .Any(group => group
                        .Matches
                        .Any(gm => _context.Set<Prediction>()
                            .Where(prediction => prediction.Owner.Id == ownerId)
                            .Select(prediction => prediction.MatchId)
                            .Contains(gm.Id))))
            .SelectMany(tournament => tournament
                .KnockOutMatches
                .Concat(tournament
                    .Groups
                    .SelectMany(group => group
                        .Matches
                        .Where(gm => _context.Set<Prediction>()
                            .Where(prediction => prediction.Owner.Id == ownerId)
                            .Select(prediction => prediction.MatchId)
                            .Contains(gm.Id))))
                .Where(km => _context.Set<Prediction>()
                    .Where(prediction => prediction.Owner.Id == ownerId)
                    .Select(prediction => prediction.MatchId)
                    .Contains(km.Id))
                .Join(_context.Set<Prediction>().Where(p => p.OwnerId == ownerId),
                    match => match.Id,
                    p => p.MatchId, (a, b) => new MatchWithPredictionModel
                    {
                        Match = a,
                        Prediction = b,
                        Tournament = tournament
                    })
            )
            .ToArrayAsync(cancellationToken);
        return t;
    }

    public async Task<IEnumerable<Prediction>> GetOwnPredictionsByMatchIdAsync(IEnumerable<Guid> matchIds, Guid ownerId, CancellationToken cancellationToken)
    {
        return await FindWhere(p => p.Owner.Id == ownerId && matchIds.Contains(p.Match.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Prediction>> GetPredictionsByMatchIdAsync(Guid matchId, CancellationToken cancellationToken)
    {
        return await FindWhere(prediction => prediction.MatchId == matchId)
            .ToArrayAsync(cancellationToken);
    }
}
