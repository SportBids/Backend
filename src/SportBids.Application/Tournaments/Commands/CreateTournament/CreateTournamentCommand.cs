#nullable disable

using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.CreateTournament;

public class CreateTournamentCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime FinishAt { get; set; }
    public int NumberOfTeams { get; set; }
    public int NumberOfGroups { get; set; }
}
