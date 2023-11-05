using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.DeleteTournament;

/// <summary>
/// Deletes a tournament
/// </summary>
public class DeleteTournamentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
