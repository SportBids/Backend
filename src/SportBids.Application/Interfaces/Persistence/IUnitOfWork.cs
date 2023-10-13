using System;
namespace SportBids.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    ITournamentRepository Tournaments { get; set; }
    IMatchRepository Matches { get; set; }
    ITeamRepository Teams { get; set; }
    IPredictionRepository Predictions { get; set; }

    void Save();
    Task SaveAsync();
}
