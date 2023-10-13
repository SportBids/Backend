#nullable disable

using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Commands.CreateTournament;

public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTournamentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTournamentCommand command, CancellationToken cancellationToken)
    {
        var tournament = new Tournament()
        {
            Name = command.Name,
            Logo = command.Logo,
            StartAt = command.StartAt,
            FinishAt = command.FinishAt,
            Groups = CreateGroups(command.NumberOfGroups),
            Teams = CreateTeams(command.NumberOfTeams),
        };

        _unitOfWork.Tournaments.Add(tournament);
        await _unitOfWork.SaveAsync();
        return tournament.Id;
    }

    private ICollection<Team> CreateTeams(int numberOfTeams)
    {
        if (numberOfTeams < 2) throw new ArgumentOutOfRangeException(nameof(numberOfTeams));

        var teams = Enumerable
            .Range(1, numberOfTeams)
            .Select(index => new Team
            {
                Name = string.Format("Team {0}", index)
            })
            .ToList();
        return teams;
    }

    private ICollection<Group> CreateGroups(int numberOfGroups)
    {
        if (numberOfGroups < 1 || numberOfGroups > 25) throw new ArgumentOutOfRangeException(nameof(numberOfGroups));

        const int A = 65;
        var groups = Enumerable
            .Range(A, numberOfGroups)
            .Select(ch => new Group() { Name = ((char)ch).ToString() })
            .ToList();
        return groups;
    }
}
