﻿using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class TournamentRepository : RepositoryBase<Tournament, Guid>, ITournamentRepository
{
    public TournamentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tournament>> GetAsync(CancellationToken cancellationToken)
    {
        return await GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tournament>> GetPublicOnlyAsync(CancellationToken cancellationToken)
    {
        return await FindWhere(t => t.IsPublic == true)
            .ToListAsync();
    }

    public async Task<Tournament?> GetTournamentFullInfo(Guid tournamentId, CancellationToken cancellationToken)
    {
        var entity = await FindWhere(tournament => tournament.Id == tournamentId)
            .Include(tournament => tournament.Teams)
            .Include(tournament => tournament.KnockOutMatches)
            .Include(tournament => tournament.Groups)
                .ThenInclude(group => group.Matches)
            .Include(tournament => tournament.Groups)
                .ThenInclude(group => group.Teams)
            .SingleOrDefaultAsync(cancellationToken);
        return entity;
    }

    public async Task<Tournament?> GetTournamentMatchesAndTeamsAsync(Guid tournamentId, CancellationToken cancellationToken)
    {
        var entity = await FindWhere(tournament => tournament.Id == tournamentId)
            .Include(tournament => tournament.Teams)
            .Include(tournament => tournament.KnockOutMatches)
            .SingleOrDefaultAsync(cancellationToken);
        return entity;
    }

    public async Task<Tournament?> GetTournamentWithGroupsMatchesAndTeamsAsync(Guid tournamentId, CancellationToken cancellationToken)
    {
        var entity = await FindWhere(tournament => tournament.Id == tournamentId)
            .Include(tournament => tournament.Teams)
            .Include(tournament => tournament.Groups)
                .ThenInclude(group => group.Matches)
            .SingleOrDefaultAsync(cancellationToken);
        return entity;
    }
}
