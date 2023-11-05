using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Queries.GetTournaments;

public class GetAllTournamentsQuery : IRequest<IEnumerable<Tournament>>
{

}
