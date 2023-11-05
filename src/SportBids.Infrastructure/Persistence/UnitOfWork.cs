using SportBids.Application.Interfaces.Persistence;
using SportBids.Infrastructure.Persistence.Repositories;

namespace SportBids.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Lazy<ITournamentRepository> _tournamentRepository;
    private readonly Lazy<IGroupRepository> _groupRepository;
    private readonly Lazy<IMatchRepository> _matchRepository;
    private readonly Lazy<ITeamRepository> _teamRepository;
    private readonly Lazy<IPredictionRepository> _predictionRepository;

    public UnitOfWork(
        AppDbContext context,
        Lazy<ITournamentRepository> tournamentRepository,
        Lazy<IGroupRepository> groupRepository,
        Lazy<IMatchRepository> matchRepository,
        Lazy<ITeamRepository> teamRepository,
        Lazy<IPredictionRepository> predictionRepository)
    {
        _context = context;
        _tournamentRepository = tournamentRepository;
        _groupRepository = groupRepository;
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
        _predictionRepository = predictionRepository;
    }

    public ITournamentRepository Tournaments => _tournamentRepository.Value;
    public IGroupRepository Groups => _groupRepository.Value;
    public IMatchRepository Matches => _matchRepository.Value;
    public ITeamRepository Teams => _teamRepository.Value;
    public IPredictionRepository Predictions => _predictionRepository.Value;

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
