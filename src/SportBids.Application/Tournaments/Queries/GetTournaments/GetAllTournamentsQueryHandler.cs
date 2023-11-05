using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Application.Tournaments.Queries.GetTournaments;

public class GetAllTournamentsQueryHandler : IRequestHandler<GetAllTournamentsQuery, IEnumerable<Tournament>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTournamentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Tournament>> Handle(GetAllTournamentsQuery request, CancellationToken cancellationToken)
    {
        var tournaments = await _unitOfWork.Tournaments.GetAllAsync(cancellationToken);
        return tournaments;
    }
}
