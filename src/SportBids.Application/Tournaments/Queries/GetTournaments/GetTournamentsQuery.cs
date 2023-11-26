using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Queries.GetTournaments;

public class GetTournamentsQuery : IRequest<IEnumerable<Tournament>>
{
    public bool IncludeNonPublic { get; set; }
}
