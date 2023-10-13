using FluentResults;
using MediatR;

namespace SportBids.Application.Tournaments.Commands.DeleteTournament;

public class DeleteTournamentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
