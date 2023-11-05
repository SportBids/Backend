using System;
namespace SportBids.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    ITournamentRepository Tournaments { get; }
    IGroupRepository Groups { get; }
    IMatchRepository Matches { get; }
    ITeamRepository Teams { get; }
    IPredictionRepository Predictions { get; }

    void Save();
    Task SaveAsync(CancellationToken cancellationToken);
}
