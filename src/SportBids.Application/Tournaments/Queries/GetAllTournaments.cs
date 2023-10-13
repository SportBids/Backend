using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Queries;

public class GetAllTournamentsQuery : IRequest<List<Tournament>>
{

}
