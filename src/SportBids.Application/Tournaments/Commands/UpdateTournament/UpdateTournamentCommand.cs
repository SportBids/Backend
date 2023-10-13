using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.UpdateTournament;

public class UpdateTournamentCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime FinishAt { get; set; }
    public bool IsPublic { get; set; }
}
